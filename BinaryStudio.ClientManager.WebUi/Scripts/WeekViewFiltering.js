$(function () {
    window.viewModel.Filter = ko.observable();

    window.viewModel.ClickAssign = function (currentInquiry, employee) {
        OnClickAssign(employee.Id, currentInquiry.Id);
    };

    window.viewModel.ClickAddTag = function (currentInquiry, tag) {
        OnClickAddTag(tag.Id, currentInquiry.Id);
    };

    window.viewModel.ClickMoveTo = function (date, currentInquiry) {
        OnClickMoveTo(currentInquiry.Id, date);
    };

    window.viewModel.ClickAddComment = function (currentInquiry) {
        ShowModal(currentInquiry.Id);
    };

    for (var dayIndex in window.viewModel.Days) {
        var day = window.viewModel.Days[dayIndex];
        day.Id = 'day' + dayIndex;


        for (var inquiryIndex in day.Inquiries) {
            var inquiry = day.Inquiries[inquiryIndex];
            inquiry.SkypeLink = ko.computed(function () {
                return 'skype:' + inquiry.Phone + '?call';
            }, window.viewModel);

            inquiry.EmailLink = ko.computed(function () {
                return 'mailto:' + inquiry.Email+'?cc=studiobinary@gmail.com';
            }, window.viewModel);

//            inquiry.MoveToList = ko.computed(function () {
//                return new Array(
//                    new Date(this.DateString).addDays(1).format('yyyy-mm-dd'),
//                    new Date(this.DateString).addDays(2).format('yyyy-mm-dd'),
//                    new Date(this.DateString).addDays(3).format('yyyy-mm-dd'),
//                    new Date(this.DateString).addDays(7).format('yyyy-mm-dd'));
//            }, day);

            inquiry.Visible = ko.computed(function () {
                if (!this.Filter() || this.Filter().length == 0) {
                    return true;
                }
                var localInquiry = this.Inquiry;
                for (var property in localInquiry) {
                    if (localInquiry.hasOwnProperty(property) &&
                        String(localInquiry[property]).contains(this.Filter())) {
                        return true;
                    }
                }
                return false;
            }, {
                Filter: window.viewModel.Filter,
                Inquiry: inquiry
            });
        }
    }

    ko.applyBindings(window.viewModel);
});