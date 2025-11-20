$(function () {
    Webcam.set({
        width: 200,
        height: 200,
        image_format: 'jpeg',
        jpeg_quality: 90
    });

    Webcam.set('constraints', {
        facingMode: "environment"
    });
    
    Webcam.attach('#webcam');
    $("#btnCapture").click(function () {
        Webcam.snap(function (data_uri) {
            //$("#imgCapture")[0].src = data_uri;
           
            $.ajax({
                type: "POST",
                url: "CheckIn.aspx/GetCapturedImage",
                data: "{data: '" + data_uri + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                  
                    var ButtonDecode = document.getElementById("Button_DecodeQR");
                    ButtonDecode.click();
                  
                }
            });
            //$("#btnUpload").removeAttr("disabled");
        });
    });

    $("#btnUpload").click(function () {
        $.ajax({
            type: "POST",
            url: "CheckIn.aspx/SaveCapturedImage",
            data: "{data: '" + $("#imgCapture")[0].src + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
               // alert(r.d);
            }

        });
    });
});