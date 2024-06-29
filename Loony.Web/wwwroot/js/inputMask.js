        var KTInputmask = function () {

            var demos = function () {
                // date format
                $("#kt_date").inputmask("99/99/9999", {
                "placeholder": "mm/dd/yyyy",
                autoUnmask: true
                });

                // phone number format
                $("#kt_phone").inputmask("mask", {
                "mask": "(999) 999-9999"
                });

                //email address
                $("#kt_email").inputmask({
                    mask: "*{1,20}[.*{1,20}][.*{1,20}][.*{1,20}]\*{1,20}[.*{2,6}][.*{1,2}]",
                    greedy: false,
                    onBeforePaste: function (pastedValue, opts) {
                        pastedValue = pastedValue.toLowerCase();
                        return pastedValue.replace("mailto:", "");
                    },
                    definitions: {
                        '*': {
                            validator: "[0-9A-Za-z!#$%&'*+/=?^_`{|}~\-]",
                            cardinality: 1,
                            casing: "lower"
                        }
                    }
                });
            }

            return {
                // public functions
                init: function() {
                    demos();
                }
            };
        }();

        jQuery(document).ready(function() {
            KTInputmask.init();
        });