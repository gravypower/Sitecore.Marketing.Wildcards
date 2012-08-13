using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Data.Templates;
using System.Reflection;
using Sitecore.Data.Managers;

namespace Sitecore.Marketing.Wildcards.Pipelines
{
    public class WildcardProcessor : Sitecore.Pipelines.HttpRequest.HttpRequestProcessor
    {
        private Dictionary<ID, IFindItem> findItemImplementation = new Dictionary<ID, IFindItem>();

        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Process(Sitecore.Pipelines.HttpRequest.HttpRequestArgs args)
        {
            var wildcardItem = Sitecore.Context.Item;

            if (wildcardItem == null)
                return;

            if (wildcardItem.Template == null)
                return;

            TemplateItem templateItem = wildcardItem.Database.Templates[Configuration.Settings.Tokenize.Wildcard];

            if (templateItem == null)
                return;

            Template template = TemplateManager.GetTemplate(wildcardItem);

            if (template == null)
                return;

            // if we are processing an 'wildcard' item or an item that has a base as wildcard
            if (!(template.ID == templateItem.ID || template.DescendsFrom(templateItem.ID)))
                return;

            var ts = WildcardManager.Provider.GetWildcardUrl(Sitecore.Context.Item, Sitecore.Context.Site);

            //what is the url i am currently at?
            var url = Sitecore.Context.RawUrl;

            //get the a list of the new token objects
            var tokens = ts.FindTokenValues(url);

            //create a stack to store the items retrieved from the tokens and data sources 
            //using a stack as the order madders in case we are trying to work out what item the the one we want if multiple on the same name are found
            var wildcardProcessorItemStack = new Stack<WildcardInformation>();
            for (int i = 0; i < tokens.Count; i++)
            {
                var wildCardInformation = new WildcardInformation();
                wildCardInformation.WildcardItem = wildcardItem;
                wildcardItem = wildcardItem.Parent;
                var t = tokens[i];
                if (!String.IsNullOrEmpty(t.DataSourceString))
                {
                    //get the data source item
                    var dataItem = Sitecore.Context.Database.GetItem(new ID(t.DataSourceString));

                    Item[] items = null;
                    var userDefaultItemFind = true;

                    if (!String.IsNullOrEmpty(t.CustomFindMethodString))
                    {
                        IFindItem findItem;
                        if (!findItemImplementation.ContainsKey(wildcardItem.ID))
                        {
                            var m = t.CustomFindMethodString.Split(',');
                            Assembly assembly = Assembly.Load(m[1]);
                            Type type = assembly.GetType(m[0]);
                            Activator.CreateInstance(type);
                            findItem = Activator.CreateInstance(type) as IFindItem;
                            findItemImplementation.Add(wildcardItem.ID, findItem);
                        }
                        else
                        {
                            findItem = findItemImplementation[wildcardItem.ID];
                        }

                        try
                        {
                            items = findItem.FindItems(MainUtil.DecodeName(t.TokenValue));

                            wildCardInformation.Query = t.CustomFindMethodString;
                            //no error so dont use default
                            userDefaultItemFind = false;
                        }
                        catch (Exception methodException)
                        {
                            Sitecore.Diagnostics.Log.Error(string.Format("WildcardProcessor, error when running find : '{0}', Exception: '{1}'", t.CustomFindMethodString, methodException.InnerException), typeof(WildcardProcessor));
                            userDefaultItemFind = true;
                        }
                    }

                    if(userDefaultItemFind)
                    {
                        //find the item
                        var fastQuery = String.Format("fast:{0}//*[@@name = '{1}']", dataItem.Paths.FullPath, MainUtil.DecodeName(t.TokenValue));

                        //Get the possible items this could match from the bucket
                        items = Sitecore.Context.Database.SelectItems(fastQuery);

                        wildCardInformation.Query = fastQuery;
                    }


                    //check if we are going to switch the context or not
                    wildCardInformation.ContextSwitched = t.SwitchContextItemString == "1";

                    if (items != null)
                    {
                        var itemCount = items.Count();
                        //if there is only one match
                        if (itemCount == 1)
                        {
                            //get the first item as there should only be one
                            var item = items.First();

                            //are we switching the context? and if this is the last token
                            if (wildCardInformation.ContextSwitched && i == tokens.Count - 1)
                            {
                                //if the item is not null
                                if (item != null)
                                {
                                    //change the context
                                    Context.Item = item;
                                }
                            }
                            //add this item to the wildCardInformation if we need to use it later in the pipeline
                            wildCardInformation.Item = item;
                        }
                        else if (itemCount > 1) //other wise we found two possible matches
                        {
                            Item item = null;
                            if (wildcardProcessorItemStack.Count > 0)
                            {
                                //to work out which one we want we will need to check the last item that was added to the wildcardProcessorItemStack
                                var lastItem = wildcardProcessorItemStack.Peek();

                                //now select the item that parent is that of the last item added to the wildcardProcessorItemStack
                                item = items.SingleOrDefault(x => x.ParentID == lastItem.Item.ID);

                                //are we switching the context? and is the item is not null
                                if (wildCardInformation.ContextSwitched && item != null)
                                {
                                    //and this is the last token to be processed then
                                    if (i == tokens.Count - 1)
                                    {
                                        //change the context
                                        Context.Item = item;
                                    }
                                    //the context will sat as the wildcard item if we could not work out what item to use
                                    //this will have to be handled down the pipeline
                                }
                            }

                            wildCardInformation.Item = item;
                            wildCardInformation.Items = items.ToList();
                        }

                        wildCardInformation.TokenValue = t.TokenValue;
                        wildCardInformation.FullPath = dataItem.Paths.FullPath;
                        wildCardInformation.DataSource = t.DataSourceString;
                        wildCardInformation.TokenIndex = i;

                        //add the item to the list
                        wildcardProcessorItemStack.Push(wildCardInformation);
                    }
                }
            }

            //Add the items to a list stored in the http context so that we can get at any other items that were found due to the route
            HttpContext.Current.Items.Add("WildCardInformationStack", wildcardProcessorItemStack);
        }
    }
}
