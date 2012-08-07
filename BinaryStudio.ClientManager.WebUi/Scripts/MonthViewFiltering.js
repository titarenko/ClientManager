$(function () {
    window.viewModel.Filter = ko.observable();

    window.viewModel.Filter.subscribe(function(newValue) {
        for (var weekIndex in this.Weeks) {
            var week = window.viewModel.Weeks[weekIndex];
            for (var dayIndex in week.Days) {
                var day = week.Days[dayIndex];
                var buttonSelector = '#button-' + day.DateString;
                if ($(buttonSelector).val() == 'Less') {
                    toggleList(day,0);
                }
            }
        }
    }, window.viewModel);
        
    for (var weekIndex in window.viewModel.Weeks) {
        var week = window.viewModel.Weeks[weekIndex];
        for (var dayIndex in week.Days) {
            var day = week.Days[dayIndex];
                               
            for (var inquiryIndex in day.Inquiries) {
                var inquiry = day.Inquiries[inquiryIndex];
                
                inquiry.day = new Date(day.DateString).getDate();
                                                                                
                inquiry.Visible = ko.computed(function() {
                    if (!this.Filter() || this.Filter().length == 0 ) {
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
                    Inquiry: inquiry,
                    Day: day
                });
                    
                inquiry.isHidden = ko.computed(function() {
                    var inquiryCurrentVisibleIndex = -1;
                    for (var inquiryIndex in this.Day.Inquiries) {
                        var inquiry = this.Day.Inquiries[inquiryIndex];
                        if (inquiry.Visible()) {
                            inquiryCurrentVisibleIndex++;
                            if (this.Index==inquiryIndex)
                                break;
                        }
                    }
                    return inquiryCurrentVisibleIndex > this.ViewModel.MaxInquiriesWithoutToggling-1 ? true : false;
                },
                {
                    ViewModel: window.viewModel,
                    Day: day,
                    Index: inquiryIndex
                });
            }
                    
                
            day.visibleInquiriesCount = ko.computed(function() {
                var inquiryCurrentVisibleIndex = -1;
                for (var inquiryIndex in this.Inquiries) {
                    var inquiry = this.Inquiries[inquiryIndex];
                    if (inquiry.Visible()) {
                        inquiryCurrentVisibleIndex++;
                    }
                }
                return inquiryCurrentVisibleIndex;
            },day);
                


            day.isToggle = ko.computed(function() {
                if (this.visibleInquiriesCount()>window.viewModel.MaxInquiriesWithoutToggling-1) {
                    return true;
                }
                return false;
            },day);
        }
    }
    ko.applyBindings(window.viewModel);
});    