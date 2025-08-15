// Enhanced form validation and password toggle script
jQuery(document).ready(function() {
    // Password toggle for both forms
    setupPasswordToggle('#registerPasswordInput', '#registerPasswordToggle', '#registerPasswordIcon');
    setupPasswordToggle('#registerConfirmPasswordInput', '#registerConfirmPasswordToggle', '#registerConfirmPasswordIcon');
    setupPasswordToggle('#loginPasswordInput', '#loginPasswordToggle', '#loginPasswordIcon');
    
    // Setup password toggle for a specific input
    function setupPasswordToggle(inputSelector, toggleSelector, iconSelector) {
        var passwordInput = jQuery(inputSelector);
        var toggleButton = jQuery(toggleSelector);
        var icon = jQuery(iconSelector);
        
        if (passwordInput.length === 0) return; // Skip if element doesn't exist
        
        toggleButton.on('click', function(e) {
            e.preventDefault();
            
            if (passwordInput.attr('type') === 'password') {
                passwordInput.attr('type', 'text');
                icon.removeClass('bi-eye').addClass('bi-eye-slash');
            } else {
                passwordInput.attr('type', 'password');
                icon.removeClass('bi-eye-slash').addClass('bi-eye');
            }
        });
    }
    
    // Enhanced form validation
    function setupFormValidation(formSelector) {
        var form = jQuery(formSelector);
        if (form.length === 0) return;
        
        // Prevent form submission if validation fails
        form.on('submit', function(e) {
            if (!jQuery(this).valid()) {
                e.preventDefault();
                // Show validation messages
                jQuery(this).find(':input').each(function() {
                    if (!jQuery(this).valid()) {
                        jQuery(this).focus();
                        return false;
                    }
                });
                return false;
            }
        });
        
        // Real-time validation feedback
        form.find(':input').on('blur', function() {
            jQuery(this).valid();
        });
        
        // Clear validation messages on input
        form.find(':input').on('input', function() {
            if (jQuery(this).valid()) {
                jQuery(this).removeClass('is-invalid').addClass('is-valid');
            } else {
                jQuery(this).removeClass('is-valid').addClass('is-invalid');
            }
        });
    }
    
    // Setup validation for login form
    setupFormValidation('#loginForm');
    
    // Setup validation for register form (if exists)
    setupFormValidation('#registerForm');
});
