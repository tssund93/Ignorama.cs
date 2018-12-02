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