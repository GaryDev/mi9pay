$(function () {
    var domainPath = paymentSetting.domainPath;

    var interval = null;
    var selectMethod = null;
    var selectScanMode = null;

    var container = $("div.container");
    var qrDiv = $("#displayArea");
    var confirmBtn = $("#confirm");
    var barcodeTxt = $("#barcode");
    var radioMethod = $('input[name="PaymentMethod"]');
    var radioScanMode = $('input[name="ScanMode"]');

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
                "method": $('input[name="PaymentMethod"]:checked').val()
            },
            dataType: 'json',
            success: function (data) {
                container.hideLoading();
                qrDiv.empty();
                if (data.img) {
                    qrDiv.append(data.img);
                    longPolling();
                } else if (data.return_msg != "OK") {
                    qrDiv.append("<span>" + data.return_msg + "</span>");
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

    radioMethod.click(function (evt) {
        var currentVal = evt.target.value;
        if (selectMethod == currentVal) {
            return false;
        }
        selectMethod = evt.target.value;

        confirmBtn.removeClass("btn-disabled");
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
                    "method": $('input[name="PaymentMethod"]:checked').val()
                },
                dataType: 'json',
                success: function (data) {
                    container.hideLoading();
                    if (data && data.return_code == "SUCCESS") {
                        if (data.return_url && data.return_url != null)
                            window.location = data.return_url;
                    } else if (data && data.return_code == "FAIL") {
                        qrDiv.empty();
                        qrDiv.append("<span>" + data.return_msg + "</span>");
                    }
                },
                error: function () {
                    container.hideLoading();
                }
            });
        }
    });
    
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
                success: function (data) {
                    if (data && data.return_code == "SUCCESS") {
                        if (data.return_url && data.return_url != null)
                            window.location = data.return_url;
                        else
                            longPolling();
                    } else if (data && data.return_code == "FAIL") {
                        qrDiv.empty();
                        qrDiv.append("<span>" + data.return_msg + "</span>");
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