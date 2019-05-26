using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;


//WebBrowserコントロールにNewWindow2イベントを実装する
//http://main.tinyjoker.net/Tech/CSharp/WebBrowser%A5%B3%A5%F3%A5%C8%A5%ED%A1%BC%A5%EB%A4%CBNewWindow2%A5%A4%A5%D9%A5%F3%A5%C8%A4%F2%BC%C2%C1%F5%A4%B9%A4%EB.html

public class WebBrowserEx : WebBrowser{

    #region NewWindow2 イベント関連

    private AxHost.ConnectionPointCookie cookie;
    private WebBrowser2EventHelper helper;

    [DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    [DispIdAttribute(200)]
    public object Application
    {
        get
        {
            if (this.ActiveXInstance == null)
            {
                throw new AxHost.InvalidActiveXStateException("Application", AxHost.ActiveXInvokeKind.PropertyGet);
            }
            return (this.ActiveXInstance as IWebBrowser2).Application;
        }
    }

    [DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    [DispIdAttribute(552)]
    public bool RegisterAsBrowser
    {
        get
        {
            if (this.ActiveXInstance == null)
            {
                throw new AxHost.InvalidActiveXStateException("RegisterAsBrowser", AxHost.ActiveXInvokeKind.PropertyGet);
            }
            return (this.ActiveXInstance as IWebBrowser2).RegisterAsBrowser;
        }
        set
        {
            if (this.ActiveXInstance == null)
            {
                throw new AxHost.InvalidActiveXStateException("RegisterAsBrowser", AxHost.ActiveXInvokeKind.PropertyGet);
            }
            (this.ActiveXInstance as IWebBrowser2).RegisterAsBrowser = value;
        }
    }

    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    protected override void CreateSink()
    {
        base.CreateSink();
        helper = new WebBrowser2EventHelper(this);
        cookie = new AxHost.ConnectionPointCookie(this.ActiveXInstance, helper, typeof(DWebBrowserEvents2));
    }

    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    protected override void DetachSink()
    {
        if (cookie != null)
        {
            cookie.Disconnect();
            cookie = null;
        }
        base.DetachSink();
    }

    public event WebBrowserNewWindow2EventHandler NewWindow2 = (o, e) => { };

    protected virtual void OnNewWindow2(WebBrowserNewWindow2EventArgs e)
    {
        NewWindow2(this, e);
    }

    private class WebBrowser2EventHelper : StandardOleMarshalObject, DWebBrowserEvents2
    {
        private WebBrowserEx parent;

        public WebBrowser2EventHelper(WebBrowserEx parent)
        {
            this.parent = parent;
        }

        public void NewWindow2(ref object ppDisp, ref bool cancel)
        {
            var e = new WebBrowserNewWindow2EventArgs(ppDisp);
            this.parent.OnNewWindow2(e);
            ppDisp = e.ppDisp;
            cancel = e.Cancel;
        }

    }
    #endregion
}

public delegate void WebBrowserNewWindow2EventHandler(object sender, WebBrowserNewWindow2EventArgs e);

public class WebBrowserNewWindow2EventArgs : CancelEventArgs
{
    public object ppDisp { get; set; }
    
    public WebBrowserNewWindow2EventArgs(object ppDisp)
    {
        this.ppDisp = ppDisp;
    }
}

[ComImport, Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public interface DWebBrowserEvents2
{
    [DispId(251)]
    void NewWindow2(
    [InAttribute(), OutAttribute(), MarshalAs(UnmanagedType.IDispatch)] ref object ppDisp,
    [InAttribute(), OutAttribute()] ref bool cancel);
}

[ComImport, Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
public interface IWebBrowser2
{
    object Application { get; }
    bool RegisterAsBrowser { get; set; }
}