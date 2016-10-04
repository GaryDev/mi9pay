$(function () {
    var domainPath = "http://localhost/mi9pay";

    var interval = null;
    var qrDiv = $("#qrCodeImg");

    $("#confirm").click(function () {
        $.ajax({
            url: domainPath + "/gateway/qrcode",
            type: "GET",
            data: {
                "method": $('input[name="PaymentMethod"]:checked').val()
            },
            dataType: 'json',
            success: function (data) {
                qrDiv.empty();
                if (data.img) {
                    qrDiv.append(data.img);
                    longPolling();
                }
            }
        });
    });

    $('input[name="PaymentMethod"]').click(function () {
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
                    } else {
                        longPolling();
                    }
                }
            });
        }, 5000);
    };

});