function banUser(postID, banReasonID, event) {
    event.preventDefault();
    $.post("/Bans/New/" + postID + "/" + banReasonID);
}

function reportUser(postID, banReasonID, event) {
    event.preventDefault();
    $.post("/Reports/New/" + postID + "/" + banReasonID);
}