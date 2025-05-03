using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RYCBEditorX.Utils;
public class MarkdownViewer : Markdig.Wpf.MarkdownViewer
{
    public MarkdownViewer()
    {
        if (Document != null && Document.Parent != null)
        {
            var ctrl = Document.Parent as FlowDocumentScrollViewer;
            ctrl.IsToolBarVisible = false;
            ctrl.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ctrl.MinZoom = 100;
            ctrl.MaxZoom = 100;
        }
    }
    protected override void RefreshDocument()
    {
        if (Document != null && Document.Parent != null)
        {
            var ctrl = Document.Parent as FlowDocumentScrollViewer;
            ctrl.IsToolBarVisible = false;
            ctrl.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ctrl.MinZoom = 100;
            ctrl.MaxZoom = 100;
        }
        base.RefreshDocument();
    }
}
