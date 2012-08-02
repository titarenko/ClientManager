$(function() {
    window.viewModel.Filter = ko.observable();
        
    window.viewModel.ClickAssign = function(currentInquiry, employee) {
        OnClickAssign(employee.Id, currentInquiry.Id);
    };
        
    window.viewModel.ClickAddTag = function(currentInquiry, tag) {
        OnClickAddTag(tag.Id, currentInquiry.Id);
    };
        
    window.viewModel.ClickMoveTo = function(date, currentInquiry) {
        OnClickMoveTo(currentInquiry.Id,date);
    };
        
    window.viewModel.ClickAddComment = function(currentInquiry) {
        ShowModal(currentInquiry.Id);
    };
        
    for (var inquiryIndex in window.viewModel.Inquiries) {
        var inquiry = window.viewModel.Inquiries[inquiryIndex];
        inquiry.SkypeLink = ko.computed(function() {
            return 'skype:' + inquiry.Phone + '?call';
        }, window.viewModel);
                
        inquiry.EmailLink = ko.computed(function() {
            return 'mailto:' + inquiry.Email;
        }, window.viewModel);
                
        inquiry.Visible = ko.computed(function() {
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

    ko.applyBindings(window.viewModel);
});