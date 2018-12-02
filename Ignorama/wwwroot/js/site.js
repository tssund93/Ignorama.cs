function banUser(postID, banReasonID, event) {
    event.preventDefault();
    $.post("/Bans/New/" + postID + "/" + banReasonID);
}