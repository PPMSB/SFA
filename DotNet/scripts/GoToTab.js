function GoToTab(pageName) {
    document.getElementById(pageName).click();
}

function controlEnter(obj, event) {
    var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
    if (keyCode == 13) {
        __doPostBack(obj, '');
        //document.getElementById(obj).click();
        return false;
    }
    else {
        return true;
    }
}

