var dateMixin = {
    filters: {
        date: function (date) {
            if (!date) return '';
            date = new Date(date + 'Z');
            return date.toLocaleString();
        },
    }
}