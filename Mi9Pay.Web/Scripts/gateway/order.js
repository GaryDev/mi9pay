$(function () {
    var domainPath = paymentSetting.domainPath;

    var interval = null;
    var selectMethod = null;
    var selectScanMode = null;
    var selectCombine = null;

    var container = $("div.container");
    var qrDiv = $("#displayArea");
    var confirmBtn = $("#confirm");
    var barcodeTxt = $("#barcode");
    //var radioMethod = $('input[name="PaymentMethod"]');
    //var radioScanMode = $('input[name="ScanMode"]');
    var radioCombine = $('input[name="PaymentCombine"]');

    //confirmBtn.addClass("btn-disabled");
    confirmBtn.hide();
    barcodeTxt.hide();

    confirmBtn.click(function () {
        if (confirmBtn.hasClass("btn-disabled"))
            return false;

        confirmBtn.addClass("btn-disabled");

        container.showLoading();
        $.ajax({
            url: domainPath + "/gateway/qrcode",
            type: "GET",
            data: {
                "method": selectMethod,
                "cid": selectCombine
            },
            dataType: 'json',
            success: function (allData) {
                container.hideLoading();
                qrDiv.empty();
                if (allData.data.img) {
                    qrDiv.append(allData.data.img);
                    longPolling();
                } else if (allData.return_msg != "OK") {
                    qrDiv.append("<span>" + allData.return_msg + "</span>");
                    if (interval)
                        clearInterval(interval);
                } else {
                    confirmBtn.removeClass("btn-disabled");
                }
            },
            error: function () {
                container.hideLoading();
                confirmBtn.removeClass("btn-disabled");
            }
        });
    });

    barcodeTxt.bind('input propertychange', function () {
        var barcode = $(this).val();
        //console.log(barcode.length + ' characters');
        if (barcode.length == 18) {
            container.showLoading();
            $.ajax({
                url: domainPath + "/gateway/barcode",
                type: "POST",
                data: {
                    "barcode": barcode,
                    "method": selectMethod,
                    "cid": selectCombine
                },
                dataType: 'json',
                success: function (allData) {
                    container.hideLoading();
                    barcodeTxt.val("").focus();
                    if (allData && allData.return_code == "SUCCESS") {
                        if (allData.data.return_url && allData.data.return_url != null)
                            window.location = allData.data.return_url;
                    } else if (allData && allData.return_code == "FAIL") {
                        qrDiv.empty();
                        qrDiv.append("<span>" + allData.return_msg + "</span>");
                    }
                },
                error: function () {
                    container.hideLoading();
                    barcodeTxt.val("").focus();
                }
            });
        }
    });

    radioCombine.click(function (evt) {
        var currentVal = evt.target.value;
        if (selectCombine == currentVal) {
            return false;
        }
        selectCombine = evt.target.value;

        qrDiv.empty();
        if (interval)
            clearInterval(interval);

        selectMethod = $(this).attr("paymethod");
        selectScanMode = $(this).attr("paymode");

        if (selectScanMode == "qrcode") {
            barcodeTxt.hide();
            confirmBtn.removeClass("btn-disabled").show();
        } else if (selectScanMode == "barcode") {
            confirmBtn.hide();
            barcodeTxt.val("").show().focus();
        }
    });
    radioCombine.filter(":checked").trigger("click");

    /*
    radioMethod.click(function (evt) {
        var currentVal = evt.target.value;
        if (selectMethod == currentVal) {
            return false;
        }
        selectMethod = evt.target.value;

        if (confirmBtn.is(":visible"))
            confirmBtn.removeClass("btn-disabled");
        else if (barcodeTxt.is(":visible")) {
            barcodeTxt.val("").focus();
        }

        qrDiv.empty();
        if (interval)
            clearInterval(interval);
    });

    
    radioScanMode.click(function (evt) {
        var currentVal = evt.target.value;
        if (selectScanMode == currentVal) {
            return false;
        }
        selectScanMode = evt.target.value;

        qrDiv.empty();
        if (interval)
            clearInterval(interval);

        if (selectScanMode == "qrcode") {
            barcodeTxt.hide();
            confirmBtn.show();
        } else if (selectScanMode == "barcode") {
            confirmBtn.hide();
            barcodeTxt.show().focus();
        }        
    });
    radioScanMode.filter(":checked").trigger("click");
    */
    
    function longPolling() {
        interval = setTimeout(function () {
            $.ajax({
                url: domainPath + "/gateway/order/polling",
                type: "GET",
                data: {
                },
                dataType: "json",
                timeout: 5000,
                error: function (error) {
                    longPolling();
                },
                success: function (allData) {
                    if (allData && allData.return_code == "SUCCESS") {
                        if (allData.data.return_url && allData.data.return_url != null)
                            window.location = allData.data.return_url;
                        else
                            longPolling();
                    } else if (allData && allData.return_code == "FAIL") {
                        qrDiv.empty();
                        qrDiv.append("<span>" + allData.return_msg + "</span>");
                        if (interval)
                            clearInterval(interval);
                    } else {
                        longPolling();
                    }
                }
            });
        }, 5000);
    };

});