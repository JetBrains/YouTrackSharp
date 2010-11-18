// Type: JetBrains.CommonControls.PresentableItem
// Assembly: JetBrains.Platform.ReSharper.UI, Version=5.1.1753.1, Culture=neutral, PublicKeyToken=1010a0d8d6380325
// Assembly location: C:\Program Files (x86)\JetBrains\ReSharper\v5.1\Bin\JetBrains.Platform.ReSharper.UI.dll

using JetBrains.Annotations;
using JetBrains.UI.RichText;
using System.Collections.Generic;
using System.Drawing;

namespace JetBrains.CommonControls
{
    public class PresentableItem : IPresentableItem, IPresentableItemImageOwner
    {
        public PresentableItem();
        public PresentableItem(string text);
        public PresentableItem(RichText richText);
        public PresentableItem(RichText richText, IList<PresentableItemImage> images);
        public PresentableItem(Image icon);
        public PresentableItem(Image icon, RichText label);
        public PresentableItem(IPresentableItem other);

        #region IPresentableItem Members

        public void Clear();

        [NotNull]
        public RichText RichText { get; set; }

        public PresentableItemImageCollection Images { get; }

        #endregion

        #region IPresentableItemImageOwner Members

        public void NotifyImageCollectionChanged();

        #endregion

        protected virtual void UpdateItem();
    }
}
