// Share functionality for SVG content
window.shareUtils = {
    async shareFlag(svgContent) {
        // Check if Web Share API is available
        if (!navigator.share) {
            alert('Share dialog is not supported on this device.');
            return;
        }

        try {
            // Convert SVG string to Blob
            const blob = new Blob([svgContent], { type: 'image/svg+xml' });
            
            // Create a File from the Blob
            const file = new File([blob], 'flag.svg', { type: 'image/svg+xml' });
            
            // Share using the Web Share API
            await navigator.share({
                files: [file],
                title: 'Generated Flag',
                text: 'Check out this procedurally generated flag!'
            });
        } catch (error) {
            if (error.name !== 'AbortError') {
                console.error('Share failed:', error);
            }
        }
    }
};
