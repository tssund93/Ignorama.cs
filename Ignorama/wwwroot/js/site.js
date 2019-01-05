function banUser(postID, banReasonID, event, callback) {
    event.preventDefault();
    $.post("/Bans/New/" + postID + "/" + banReasonID)
        .done(() => {
            if (callback) callback();
        });
}

function reportUser(postID, banReasonID, event) {
    event.preventDefault();
    $.post("/Reports/New/" + postID + "/" + banReasonID);
}

function setClipboard(value, event) {
    event.preventDefault();
    var tempInput = document.createElement("input");
    tempInput.style = "position: absolute; left: -1000px; top: -1000px";
    tempInput.value = value;
    document.body.appendChild(tempInput);
    tempInput.select();
    document.execCommand("copy");
    document.body.removeChild(tempInput);
    alert("Copied '" + tempInput.value + "' to clipboard")
}