// Type: JetBrains.ReSharper.Features.Common.TreePsiBrowser.TreeModelBrowserPresenter
// Assembly: JetBrains.ReSharper.Features.Common, Version=5.1.1753.1, Culture=neutral, PublicKeyToken=1010a0d8d6380325
// Assembly location: C:\Program Files (x86)\JetBrains\ReSharper\v5.1\Bin\JetBrains.ReSharper.Features.Common.dll

using JetBrains.Annotations;
using JetBrains.CommonControls;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Psi;
using JetBrains.TreeModels;
using JetBrains.UI.RichText;
using JetBrains.UI.TreeView;
using JetBrains.Util;

namespace JetBrains.ReSharper.Features.Common.TreePsiBrowser
{
    public class TreeModelBrowserPresenter : StructuredPresenter<TreeModelNode, IPresentableItem>
    {
        public static readonly DeclaredElementPresenterStyle FULL_NESTED_STYLE;
        public static readonly DeclaredElementPresenterStyle TYPE_MEMBER_STYLE;
        public static readonly DeclaredElementPresenterStyle TYPE_MEMBER_STYLE_WITH_CONTAINER;
        public static readonly DeclaredElementPresenterStyle TYPE_MEMBER_STYLE_WITH_FQ_CONTAINER;
        public static readonly DeclaredElementPresenterStyle KIND_EXTENSIONS_STYLE;
        public static readonly DeclaredElementPresenterStyle TYPE_ELEMENT_QUALIFY_AFTER;
        public static readonly DeclaredElementPresenterStyle TYPE_ELEMENT_QUALIFY_BEFORE;
        public static readonly TextStyle ourAdditionalInfoStyle;
        public static readonly TextStyle ourTypeStyle;
        public static readonly TextStyle ourTextStyle;
        public static readonly TextStyle ourBoldTextStyle;
        public static readonly TextStyle ourOccurenceCountStyle;
        public TreeModelBrowserPresenter();
        public bool PostfixTypeQualification { get; set; }
        public bool PostfixMemberQualification { get; set; }
        public bool DrawElementExtensions { get; set; }
        public bool ShowOccurenceCount { get; set; }

        public override void UpdateItem(object value, TreeModelNode structureElement, IPresentableItem item,
                                        PresentationState state);

        protected virtual PsiLanguageType GetPresentationLanguage(IDeclaredElement value);
        protected static void MarkInvalid(RichText richText);
        protected virtual bool IsNodeParentNatural(TreeModelNode modelNode, object childValue);
        protected virtual bool IsNaturalParent([NotNull] object parentValue, [NotNull] object childValue);

        [CanBeNull]
        protected virtual object Unwrap(object value);

        protected virtual void PresentProjectItemEnvoy(ProjectModelElementEnvoy value, IPresentableItem item,
                                                       TreeModelNode modelNode, PresentationState state);

        protected virtual void PresentDeclaredElementEnvoy(IDeclaredElementEnvoy value, IPresentableItem item,
                                                           TreeModelNode modelNode, PresentationState state);

        protected virtual void PresentDeclaredElement(IDeclaredElement value, IPresentableItem item,
                                                      TreeModelNode modelNode, PresentationState state);

        protected virtual void PresentObject(object value, IPresentableItem item, TreeModelNode modelNode,
                                             PresentationState state);

        protected virtual void PresentSection(TreeSection value, IPresentableItem item, TreeModelNode modelNode,
                                              PresentationState state);

        protected virtual void PresentSeparator(TreeSeparator value, IPresentableItem item, TreeModelNode modelNode,
                                                PresentationState state);

        protected virtual void PresentModuleReference(IModuleReference value, IPresentableItem item,
                                                      TreeModelNode modelNode, PresentationState state);

        protected virtual void PresentAssembly(IAssembly value, IPresentableItem item, TreeModelNode modelNode,
                                               PresentationState state);

        protected virtual void PresentProjectItem(IProjectItem value, IPresentableItem item, TreeModelNode modelNode,
                                                  PresentationState state);

        protected virtual void PresentNamespace(INamespace value, IPresentableItem item, TreeModelNode modelNode,
                                                PresentationState state);

        protected virtual void PresentTypeElement(ITypeElement value, IPresentableItem item, TreeModelNode modelNode,
                                                  PresentationState state);

        protected virtual void PresentTypeMember(ITypeMember value, IPresentableItem item, TreeModelNode modelNode,
                                                 PresentationState state);

        protected virtual void PresentTypeOwner(ITypeOwner value, IPresentableItem item, TreeModelNode modelNode,
                                                PresentationState state);

        protected virtual void PresentSpecialElement(Pair<object, ISpecialElementFinder> value, IPresentableItem item,
                                                     TreeModelNode structureElement, PresentationState state);

        protected virtual void HighlightDeclaredElement(IDeclaredElement declaredElement, PsiLanguageType languageType,
                                                        IPresentableItem item, TreeModelNode modelNode,
                                                        DeclaredElementPresenterMarking marking);

        protected virtual void AppendOccurencesCount(IPresentableItem item, TreeModelNode node);
        protected void AppendOccurencesCount(IPresentableItem item, TreeModelNode node, string itemText);
    }
}
