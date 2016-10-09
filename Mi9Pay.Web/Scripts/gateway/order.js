$(function () {
    var domainPath = "http://localhost/mi9pay";

    var interval = null;
    var selectMethod = null;

    var container = $("#payOrderContainer");
    var qrDiv = $("#qrCodeImg");
    var confirmBtn = $("#confirm");
    var radioMethod = $('input[name="PaymentMethod"]');

    confirmBtn.addClass("btn-disabled");
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

    function longPolling() {
        interval = setTimeout(function () {
            $.ajax({
                url: domainPath + "/gateway/order/polling",
                type: "GET",
                data: {
                    "invoice": $("#invoice").text()
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