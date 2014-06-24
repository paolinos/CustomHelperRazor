/**
    
    This script is for "DemoForm.cshtml" view.

    LastUpdated:    2014/06/19

*/
$(document).ready(function () {
    
    //  Buttons
    $btnSave = $('#btnSave');
    $btnEnableAjax = $('#btnEnableAjax');

    //  Save/Error Message
    $msgResult = $('#msgResult');

    //  Error Server Validation
    $errorServerMsg = $('#errorServerMsg');

    /**
        Wait time, and call callback.
    */
    var waitTime = function (callback, time) {
        var interval;
        var waitingTime = function () {
            clearInterval(interval);
            if (callback !== undefined) {
                callback();
            }
        }
        interval = setInterval(waitingTime, time);
    }

    /**
        Show message for a specific time
    */
    var showMessage = function (error, msg) {
        
        if (error) {
            $msgResult.removeClass().addClass("danger");
        } else {
            $msgResult.removeClass().addClass("success");
        }
        $msgResult.html(msg);
        $msgResult.show();
        waitTime(function () {
            $msgResult.hide();
        },10000);
    }

    var form = null;

    $btnEnableAjax.click(function (e) {
        if (form == null) {
            /*
                Two ways to use CustomHelperForm.
                by default it's not necesarry send any parameter.

                but if we need specificate the main, we can send the name, like second one.
            */
            form = new CustomHelperForm();
            //form = new CustomHelperForm({ form: '#main' });
            
            $btnEnableAjax.prop('value', 'Disable Ajax');
            $errorServerMsg.hide();
        } else {
            form.Destroy();
            form = null;
            $btnEnableAjax.prop('value', 'Enable Ajax');
            $errorServerMsg.show();
        }
    });


    $btnSave.click(function (e) {
        if (form != null) {

            //  Validate form
            form.validate();

            //  Check if form is Valid or not
            var valid = form.isValid();

            //  if valid we can send the data by ajax, else the CustomHelperForm will show the validations messages.
            if (valid)
            {
                //  Get data to send
                var data = form.getData();

                AjaxManager.I().Call(
                    {
                    type: "POST",
                    data: data,
                    url: "/Home/DemoFormAjax",
                    success: function (data)
                        {
                            try {
                                data = JSON.parse(data);
                                if (data.error) {
                                    showMessage(true, "Error: " + data.msg);
                                } else {
                                    form.clearFields();
                                    showMessage(false, "Saved");
                                }
                            } catch (e) {
                                //  IE6 not support JSON
                                //  But with "Scripts/json/json2.js" you include a JSON parser for IE6 too.
                                showMessage(true, "There are a problem with the server connection. <br> Or Your browser is IE6 or older. <br> You can include json2.js script, to fix this.");
                            }
                        }
                    });
            }
        }
    });

    //  Save without Ajax
    if (saved) {

        showMessage(false, "Saved");
    }
});