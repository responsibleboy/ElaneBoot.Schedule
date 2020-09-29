// JS`正则表达式`获取地址栏url参数：
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); // 构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); // 匹配目标参数
    if (r != null) return decodeURIComponent(r[2]);
    return ''; // 返回参数值
}
function httpGet(params) {
    $.ajax({
        url: params.url,
        type: 'GET',
        data: null,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        async: true,
        timeout: 0,
        success: function (r) {
            if (params.success) {
                params.success(r);
            }
            else {
                alert(r.Msg);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('error');
            //alert(textStatus);
            //alert(errorThrown);
            //alert(XMLHttpRequest.readyState);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //alert('complete');
            //alert(textStatus);
            //alert(XMLHttpRequest.readyState);
        }
    });
}
function httpDelete(params) {
    $.ajax({
        url: params.url,
        type: 'DELETE',
        data: null,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        async: true,
        timeout: 0,
        success: function (r) {
            if (params.success) {
                params.success(r);
            }
            else {
                alert(r.Msg);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('error');
            //alert(textStatus);
            //alert(errorThrown);
            //alert(XMLHttpRequest.readyState);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //alert('complete');
            //alert(textStatus);
            //alert(XMLHttpRequest.readyState);
        }
    });
}
function httpPost(params) {
    $.ajax({
        url: params.url,
        type: 'POST',
        data: params.data,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        async: true,
        timeout: 0,
        success: function (r) {
            if (params.success) {
                params.success(r);
            }
            else {
                alert(r.Msg);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('error');
            //alert(textStatus);
            //alert(errorThrown);
            //alert(XMLHttpRequest.readyState);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //alert('complete');
            //alert(textStatus);
            //alert(XMLHttpRequest.readyState);
        }
    });
}
function httpPut(params) {
    $.ajax({
        url: params.url,
        type: 'PUT',
        data: params.data,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        async: true,
        timeout: 0,
        success: function (r) {
            if (params.success) {
                params.success(r);
            }
            else {
                alert(r.Msg);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('error');
            //alert(textStatus);
            //alert(errorThrown);
            //alert(XMLHttpRequest.readyState);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //alert('complete');
            //alert(textStatus);
            //alert(XMLHttpRequest.readyState);
        }
    });
}