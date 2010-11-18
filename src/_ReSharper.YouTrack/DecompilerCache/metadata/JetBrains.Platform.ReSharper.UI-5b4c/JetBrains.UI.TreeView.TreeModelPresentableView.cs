// Type: JetBrains.UI.TreeView.TreeModelPresentableView
// Assembly: JetBrains.Platform.ReSharper.UI, Version=5.1.1753.1, Culture=neutral, PublicKeyToken=1010a0d8d6380325
// Assembly location: C:\Program Files (x86)\JetBrains\ReSharper\v5.1\Bin\JetBrains.Platform.ReSharper.UI.dll

using JetBrains.CommonControls;
using JetBrains.TreeModels;
using System.ComponentModel;
using System.Drawing;

namespace JetBrains.UI.TreeView
{
    public class TreeModelPresentableView : TreeModelView
    {
        public TreeModelPresentableView(TreeModel model, ITreeViewController controller);
        public TreeModelPresentableView(ITreeViewController controller);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual StructuredPresenter<TreeModelNode, IPresentableItem> Presenter { get; set; }

        public virtual TreeModelViewColumn ModelColumn { get; }
        protected override void Initialize();
        protected override void SetupColumnWidth();
        protected override void InitializeCells(TreeModelViewNode viewNode, TreeModelNode modelNode);

        protected override void UpdateNodeCells(TreeModelViewNode viewNode, TreeModelNode modelNode,
                                                PresentationState state);

        public Rectangle GetNodeBounds(TreeModelViewNode node);
    }
}
