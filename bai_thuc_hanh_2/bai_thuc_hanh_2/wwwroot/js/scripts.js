/*!
 * Start Bootstrap - SB Admin Template
 * JavaScript for sidebar toggle functionality
 */
(function() {
    'use strict';
    window.addEventListener('DOMContentLoaded', function() {
        // Toggle the side navigation
        const sidebarToggle = document.body.querySelector('#sidebarToggle');
        if (sidebarToggle) {
            sidebarToggle.addEventListener('click', function(event) {
                event.preventDefault();
                document.body.classList.toggle('sb-sidenav-toggled');
                localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
            });
        }

        // Restore sidebar toggle state from localStorage
        if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
            document.body.classList.add('sb-sidenav-toggled');
        }
    });
})();

