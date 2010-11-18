using System;
using DevExpress.XtraTreeList;
using JetBrains.CommonControls;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;

namespace YouTrackPowerToy
{
    public class YouTrackIssueView: TreeModelPresentableView 
    {
       
        protected override void Initialize()
        {
            base.Initialize();
            ModelColumn.FieldName = "Description";
            ModelColumn.Name = "Description";
            ModelColumn.Caption = "Description";
          
            AddColumn();
            ModelColumn.FieldName = "Summary";
            ModelColumn.Name = "Summary";
            ModelColumn.Caption = "Summary";
          
        }



        protected override void InitializeCells(TreeModelViewNode viewNode, TreeModelNode modelNode)
        {
            viewNode.SetValue(ModelColumn, new PresentableItem());
        }

        protected override void UpdateNodeCells(TreeModelViewNode viewNode, TreeModelNode modelNode, PresentationState state)
        {
            var presentableItem = viewNode.GetCellValue(ModelColumn) as IPresentableItem;
            if (presentableItem == null)
                return;
            presentableItem.Clear();
            Presenter.UpdateItem(modelNode, presentableItem, state);
            InvalidateNode(viewNode);
        }

     
        public YouTrackIssueView(TreeModel model, ITreeViewController controller) : base(model, controller)
        {

        }

        public YouTrackIssueView(ITreeViewController controller) : base(controller)
        {
        }
    }
}