$(function() {
    window.viewModel.Filter = ko.observable('');
    for (var categoryIndex in window.viewModel.Categories) {
        var category = window.viewModel.Categories[categoryIndex];
        for (var inquiryIndex in category.Inquiries) {
            var inquiry = category.Inquiries[inquiryIndex];
            inquiry.Tag = category.Tag.Name;
            inquiry.InquiryDetailsUrl = window.viewModel.InquiryDetailsUrl + '/' + inquiry.Id;
            inquiry.Visible = ko.computed(function() {
                if (this.Filter().length == 0) {
                    return true;
                }
                var localInquiry = this.Inquiry;
                for (var property in localInquiry) {
                    if (localInquiry.hasOwnProperty(property) &&
                        property != 'InquiryDetailsUrl' &&
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