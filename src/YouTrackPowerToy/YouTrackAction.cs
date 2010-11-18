using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.IDE;
using JetBrains.IDE.TreeBrowser;
using JetBrains.TreeModels;
using JetBrains.UI.Application;
using YouTrackSharp;

namespace YouTrackPowerToy
{
    
        [ActionHandler]
        public class YouTrackAction : IActionHandler
        {
            public const string YouTrackBrowserWindowID = "YouTrackBrowserWindowID";

            public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
            {
                // It's always allowed. We don't need a solution present
                return context.CheckAllNotNull(DataConstants.SOLUTION);
            }

            public void Execute(IDataContext context, DelegateExecute nextExecute)
            {
                var solution = context.GetData(DataConstants.SOLUTION);


                if (solution == null)
                {
                    // Do something...shit! 

                }
                else
                {
                    using (var searchBox = new SearchBox())
                    {
                        if (searchBox.ShowDialog(UIApplicationShell.Instance.MainWindow) == DialogResult.OK)
                        {
                            var model = new TreeSimpleModel();
                            
                            
                            Issue parent = new Issue {Summary = "Issues"};

                            model.Insert(null, parent);

                            var youtrackClient = new YouTrackConnection("youtrack.jetbrains.net");

                            //var issues = youtrackClient.GetIssues(searchBox.SearchString);

                            //foreach(var issue in issues)
                            //{
                            //    model.Insert(parent, issue);                                        
                            //}
                            
                            var controller = new YouTrackTreeViewController(solution, model);
                            var browserPanel = new YouTrackTreeModelPanel(controller);
                            var browser = TreeModelBrowser.GetInstance(solution);

                            browser.Show(YouTrackBrowserWindowID, controller, browserPanel);
                        }
                    }

                }
            }
        }
}




