/**
    Validate
    Clase que trabaja en conjunto con OcaHelpers, para chequear las validaciones de los campos desde el cliente.

    Framework requeridos: jQuery.

    Parametros opcionales:
        form:       Nombre del formulario o nombre del tag.Ej.: '#formId'
        errorDiv:   Nombre del error div, por si queremos agregar los errores en un 
*/
function CustomHelperForm(params) {
    var t = this;
    var $form = null;
    var $errorDiv = null;

    if (params != undefined) {
        if (params.form != undefined)
            $form = $(params.form);

        if (params.errorDiv != undefined)
            $errorDiv = $('#' + params.errorDiv);
    }

    var isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox') > 1 ? true : false;
    if ($form == null)
        $form = $('form');

    var errArray = [];

    //  Add message error to array
    var addError = function (mesg) {
        errArray[errArray.length] = mesg;
    }

    /**
        Remove text inside text
    */
    var removeTextFrom = function (str, strToRemove) {
        var posStart = str.indexOf(strToRemove);
        var newStr = str.substring(0, posStart);
        newStr += str.substring(posStart + strToRemove.length, str.length);
        return newStr;
    }

    /**
        Se leen las validaciones del objeto y se verifican los errores
    */
    var readValidations = function (htmlObject, msgError) {
        if (htmlObject == null || htmlObject == undefined)
            return;
        
        var id = htmlObject.name;
        var error = false;
        var errorType = 0;
        /*
            errorType = 0;  - No hay error
            errorType = 1;  - Requerido
            errorType = 2;  - Min or Max Lenght
            errorType = 3;  - msgError - External Error
            errorType = 4;  - Server regular expression
            errorType = 5;  - Range Error
        */
        /*
            data-val            = true
            data-val-required   = "Mensaje de error de texto requerido"
            data-val-date       = "Mensaje de error de fecha invalida"
            data-val-rule       = "Mensaje de error del Atributo del lado de Servidor"
            data-val-range      = "Mensaje de error de Rango invalido, para numeros"
        */

        $parent = $(htmlObject.parentNode);
        $validateMsg = $parent.children('[data-valmsg-for="' + id + '"]');

        //  Read values
        var requiredErrorMsg = htmlObject.getAttribute('data-val-required');
        var minLength = htmlObject.getAttribute('data-val-length-min');
        var maxLength = htmlObject.getAttribute('data-val-length-max');
        var lengthErrorMsg = htmlObject.getAttribute('data-val-length');

        //  Esto proviene de los atributos customs.
        var ruleReg = htmlObject.getAttribute('data-val-rule-reg');
        var ruleErrorMsg = htmlObject.getAttribute('data-val-rule');

        var rangeMin = htmlObject.getAttribute('data-val-range-min');
        var rangeMax = htmlObject.getAttribute('data-val-range-max');
        var rangeErrorMsg = htmlObject.getAttribute('data-val-range');

        /*
        var minRange = htmlObject.getAttribute('minRange');
        var maxRange = htmlObject.getAttribute('maxRange');
        var type = htmlObject.getAttribute('type');
        type = type == null ? 'text' : type;
        */

        requiredErrorMsg = requiredErrorMsg == null ? "" : requiredErrorMsg;


        if (requiredErrorMsg != "") {

            if (htmlObject.value.length <= 0) {
                error = true;
                errorType = 1;
            }
        }


        if (errorType == 0) {

            if (minLength != null) {
                if (htmlObject.value.length < minLength) {
                    error = true;
                    errorType = 2;
                }
            }
            if (maxLength != null) {
                if (htmlObject.value.length > maxLength) {
                    error = true;
                    errorType = 2;
                }
            }
        }

        if (errorType == 0) {
            if (ruleReg != null & htmlObject.value!="") {
                var regEx = new RegExp(ruleReg);
                if (!regEx.test(htmlObject.value)) {
                    error = true;
                    errorType = 4;
                }
            }
        }

        if (errorType == 0) {

            var val = Number(htmlObject.value);
            if (rangeMin != null) {
                if (val < Number(rangeMin)) {
                    error = true;
                    errorType = 5;
                }
            }
            if (rangeMax != null) {
                if (val > Number(rangeMax)) {
                    error = true;
                    errorType = 5;
                }
            }
        }

        if (msgError != undefined) {
            error = true;
            errorType = 3;
        }

        //  Class to show error in input
        var classError = "input-validation-error";
        var classErrorAdded = htmlObject.className.indexOf(classError);

        if (error) {
            addError(id);

            var errorMsg = "";
            if (errorType == 1) {
                errorMsg = requiredErrorMsg;
            } else if (errorType == 2) {
                errorMsg = lengthErrorMsg;
            } else if (errorType == 3) {
                errorMsg = msgError;
            } else if (errorType == 4) {
                errorMsg = ruleErrorMsg;
            } else if (errorType == 5) {
                errorMsg = rangeErrorMsg;
            }

            $validateMsg.show().html(errorMsg);

            if (classErrorAdded == -1) {
                htmlObject.className = htmlObject.className + " " + classError;
            }
        }
        else {
            $validateMsg.hide().html("");
            htmlObject.className = removeTextFrom(htmlObject.className, classError);
        }
    }

    /**
        Esto es para mostrar los errores.
        Se reemplazan los "." por los "_", de la misma manera que lo hacen los browsers y se revisa la validacion.
    */
    var checkValidationField = function (id, message) {

        id = id.replace(".", "_");
        var field = document.getElementById(id);
        readValidations(field, message);
    }

    /**
        Obtenemos la lista de campos a validar
    */
    var getFields = function () {
        //  Using .NET Validation
        return $form.find("[data-val='true']");
    }

    /**
        Agregamos mascara. Se validan los datos y solo se permiten caracteres que vienen desde el servidor.
    */
    var checkDataValRule = function () {
        var listValidate = getFields();
        $.each(listValidate, function (key, htmlObject) {

            var dataValRule = htmlObject.getAttribute('data-val-rule');
            var dataValRuleReg = htmlObject.getAttribute('data-val-rule-reg');
            var dataValRuleMask = htmlObject.getAttribute('data-val-rule-mask');

            if (dataValRuleMask != null) {
                var expression = dataValRuleMask;

                //$(htmlObject).off().bind('keypress', function (event) {
                $(htmlObject).bind('keypress', function (event) {
                    var regex = new RegExp(expression);

                    /*
                        9 = Tab
                        8 = Borrar/Backspace
                        46= Suprimir/Delete
                        13= Enter
                        35= Fin/End
                        36= Inicio
                        37= Flecha Izquierda/ Arrow Left
                        39= Flecha Derecha/ Arrow Rigth    
                    */
                    if (isFirefox) {
                        if(event.keyCode == 46)
                            return true;
                    }

                    if (event.keyCode == 9 || event.keyCode == 8 || event.keyCode == 13 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 36 || event.keyCode == 35)
                        return true;

                    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                    if (!regex.test(key)) {
                        event.preventDefault();
                        return false;
                    }
                    return true;
                });
            }
        });
    }
    checkDataValRule();

    /**
        Corre la validacion y nos guardamos los errores.
    */
    this.validate = function () {
        errArray = [];
        var listValidate = getFields();
        $.each(listValidate, function (key, htmlObject) {
            //  Check validation html object.
            readValidations(htmlObject);
        });

        if ($errorDiv != null) {
            $errorDiv.children().hide();

            for (var i = 0; i < errArray.length; i++) {
                $errorDiv.find("#" + errArray[i]).show();
            }
        }
    }

    /**
        Revisa la lista de errores para saber si existe algun campo valido.
    */
    this.isValid = function () {
        if (errArray.length > 0)
            return false;
        return true;
    }

    /**
        Es para poder mostrar los errores que nos vienen desde el servidor por ajax.
    */
    this.showError = function (errorList) {
        var tmpObj;

        if (errorList == null || errorList == undefined)
            return;

        for (var i = 0; i < errorList.length; i++) {
            tmpObj = errorList[i];
            checkValidationField(tmpObj.field, tmpObj.message);
        }
    }

    /**
        Obtener el actual formulario que se esta validando.
    */
    this.getCurrentForm = function () {
        return $form;
    }

    /**
        Se limpian los campos
    */
    this.clearFields = function () {

        $form.find(':input').each(function () {

            var input = $(this);
            var inputType = input.attr('type');

            if (inputType == "select-one")
            {   //  Clear Dropdownlist
                input.val($("#target option:first").val());
            }
            else if (inputType == "radio" || inputType == "checkbox")
            {   //  Clear RadioButtons and Checkbox
                
                input.removeAttr('checked');
            }
            else if (inputType != "submit" && inputType != "button")
            {   //  Clear Texts, TextAreas
                input.val("");
            }
        });

    }
    this.getData = function () {
        return $form.serialize();
    }
    this.Destroy = function () {
        $form.unbind("submit", submitEvent);
        $form = null;
    }

    this.success = null;
    this.error = null;

    var submitEvent = function (e) {
        e.preventDefault();
        t.validate();

        if (t.isValid()) {
            var formData = $form.serialize();
            if (t.success != null) {
                t.success(formData);
            }
        } else {
            if (t.error != null) {
                t.error();
            }
        }
    };


    var initForm = function () {

        $form.submit(submitEvent);
    }
    initForm();
}