

//validate định dạng file
var listAlowImgEx = 'jpg, jpeg, gif, png,JPG,JPEG,GIF,PNG';
function check_extension(imgData, btn) {
    imgData = imgData.split('\\').pop();
    IsValidExtension(imgData);

    $('#' + btn).click(function () {
        IsValidExtension(imgData);
    });
}

function IsValidExtension(imgData) {
    var extension = imgData.substr((imgData.lastIndexOf('.') + 1));
    if (extension != null && extension.trim() != "") {
        if (listAlowImgEx.indexOf(extension.toLowerCase()) == -1) {
            alert("Đuôi ảnh không hợp lệ. chỉ nhấp nhận " + listAlowImgEx);
            $('.fileupload-exists').click();
            return false;
        }
    }

}
//validate ngày tháng năm từ input
function check_date(a) {
    re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
    if (a.value.trim() != '' && !a.value.trim().match(re)) {
        alert("Không đúng định dạng ngày/tháng/năm: " + a.value.trim());
        form.startdate.focus();
        return false;
    }
    return true;
}

