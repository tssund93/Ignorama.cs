// this code is scary and needs to be rewritten or something, but for now it works
$(document).ready(function () {
    $(function () {
        $('#uploadForm').submit(function () {
            $('<input type="hidden" name="javascript" value="yes" />').appendTo($(this));
            var iframeName = ('iframeUpload');
            var iframeTemp = $('<iframe id="uploadframe" name="' + iframeName + '" src="about:blank" />');
            iframeTemp.css('display', 'none');
            $('body').append(iframeTemp);
            /* submit the uploadForm */
            $(this).attr({
                action: '/Home/UploadFile',
                method: 'post',
                enctype: 'multipart/form-data',
                encoding: 'multipart/form-data',
                target: iframeName
            });
            var count = 0;
            var timeout = 60;
            var postcontents = document.getElementById('postfield').value;
            var uploadingtext = postcontents + " Uploading file";
            var uploadinterval = setInterval(function () {
                var error = document.getElementById("uploadframe").contentWindow.error;
                var fileUri = document.getElementById("uploadframe").contentWindow.fileUri;
                if (typeof error !== 'undefined' || count > timeout) {
                    if (count > timeout) {
                        document.getElementById('postfield').value = postcontents;
                        alert("Upload timed out.");
                        document.getElementById("submitbutton").disabled = false;
                    }
                    document.getElementById('postfield').value = postcontents;
                    clearInterval(uploadinterval);
                    iframeTemp.remove();
                    document.getElementById("submitbutton").disabled = false;
                }
                else {
                    document.getElementById('postfield').value = uploadingtext + ".";
                    uploadingtext = document.getElementById('postfield').value;
                }
                $('input[name="javascript"]').remove();
                inputLength = 0;
                inputLength += $('input[name="File"]').val().length;
                if (0 < inputLength && error === 'none') {
                    $('input[name="File"]').val('');
                    document.getElementById('postfield').value = postcontents;
                    temp = $('#filename').val().split(".");
                    extension = temp[temp.length - 1];
                    if (extension === 'webm') {
                        document.getElementById('postfield').value += "[webm]" + fileUri + "[/webm]";
                    }
                    else {
                        document.getElementById('postfield').value += "[img]" + fileUri + "[/img]";
                    }
                    var input = $("#postfield");
                    var len = input.val().length;
                    input[0].focus();
                    input[0].setSelectionRange(len, len);
                    document.getElementById("submitbutton").disabled = false;
                }
                else if (error !== 'none' && typeof error !== 'undefined') {
                    alert(error);
                    $('input[name="File"]').val('');
                    document.getElementById("submitbutton").disabled = false;
                }
                count++;
            }, 1000);
        });
    });
});

function updateFilename() {
    temp = $('#upload').val().split(".");
    extension = temp[temp.length - 1];
    $('#filename').val(window.performance.now().toString().replace('.', '') +
        '.' + extension);
    document.getElementById("submitbutton").disabled = true;
    $('#uploadsubmit').click();
};