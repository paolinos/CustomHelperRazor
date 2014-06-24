function Call(obj) {
    obj = obj[0];

    var xmlhttp;
    if (window.XMLHttpRequest) {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {
        // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {

        if (xmlhttp.readyState == 4) {
            //  COMPLETE

            if (xmlhttp.status == 200) {
                //  OK
                //  responseText || responseXML
                /*
                if(responseType == ''  || responseType == 'txt')
                    obj.complete(obj, xmlhttp.responseText, false);
                else
                    obj.complete(obj, xmlhttp.response, false);
                */

                obj.complete(obj, xmlhttp.responseText, false);

            } else {
                //  ERROR
                obj.complete(obj, "", true)
            }
            
        }
    }
    
    xmlhttp.open(obj.type, obj.url, true);
    xmlhttp.responseType = obj.dataType;

    xmlhttp.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xmlhttp.setRequestHeader("If-Modified-Since", "Sat, 1 Jan 2005 00:00:00 GMT");

    if (obj.data != "") {
        
        //Send the proper header information along with the request
        
        

        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xmlhttp.setRequestHeader("Content-length", obj.data.length);
        xmlhttp.setRequestHeader("Connection", "close");

        xmlhttp.send(obj.data);
    } else {
        xmlhttp.send();
    }
};

function ObjCall(params,complete, position) {
    if (params == undefined) return;
    //  Read Data
    this.type = params.type == undefined ? "GET" : params.type;
    this.url = params.url == undefined ? "" : params.url;
    this.data = params.data == undefined ? "" : params.data;
    this.dataType = params.dataType == undefined ? "" : params.dataType;
    this.success = params.success == undefined ? null : params.success;
    this.error = params.error == undefined ? null : params.error;
    this.complete = complete;
    this.pos = position;
}

function AjaxManager() {
    var t = this;
    var count = 0;
    var countCalling = 0;
    var maxCalls = 2;
    var callers = [];
    var waiting = [];

    var Add = function (array, obj) {
        array[array.length] = obj;
    }
    var GetObjFromArrayBy = function (array, key, value) {
        for (var i = array.length - 1; i >= 0; i--) {
            if (array[i][key] == value)
                return array[i];
        };
        return null;
    }
    var GetPosFromArrayBy = function (array, key, value) {
        for (var i = array.length - 1; i >= 0; i--) {
            if (array[i][key] == value)
                return i;
        };
        return null;
    }
    var Remove = function (array, pos) {
        return array.splice(pos, 1);
    }

    /*
        Check if can call other
    */
    var CheckToCall = function () {
        if (callers.length < maxCalls) {
            if (waiting.length <= 0) return;
            var tmpObj = Remove(waiting, 0);
            if (tmpObj != null) {
                Add(callers, tmpObj);
                new Call(tmpObj);
            }
        }
    }

    /*
        Caller complete
    */
    var AjaxComplete = function (obj, data, error) {
        var tmpPos = GetPosFromArrayBy(obj.pos);
        Remove(callers, tmpPos);

        CheckToCall();

        if(error){
            console.error("Error calling: " + obj.url );
        }
        if(obj.success != null)
            obj.success(data)

    }

    this.Call = function (params) {
        var obj = new ObjCall(params, AjaxComplete, count);
        count++;

        Add(waiting, obj);
        CheckToCall();
    }
}
AjaxManager.instance = null;
AjaxManager.I = function () {
    if (AjaxManager.instance == null)
        AjaxManager.instance = new AjaxManager();
    return AjaxManager.instance;
}