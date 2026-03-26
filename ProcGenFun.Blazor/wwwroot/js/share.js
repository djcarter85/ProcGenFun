// Share functionality for SVG content
window.shareUtils = {
  async shareFlag(svgContent) {
    // Check if Web Share API is available
    if (!navigator.share) {
      alert("Share dialog is not supported on this device.");
      return;
    }

    try {
      // Convert SVG to PNG
      const pngBlob = await this.svgToPng(svgContent);

      // Create a File from the Blob
      const file = new File([pngBlob], "flag.png", { type: "image/png" });

      // Share using the Web Share API
      await navigator.share({
        files: [file],
        title: "Generated Flag",
      });
    } catch (error) {
      if (error.name !== "AbortError") {
        console.error("Share failed:", error);
      }
    }
  },

  async svgToPng(svgContent) {
    return new Promise((resolve, reject) => {
      // Create an image element from the SVG string
      const img = new Image();
      const blob = new Blob([svgContent], { type: "image/svg+xml" });
      const url = URL.createObjectURL(blob);

      img.onload = () => {
        // Create a canvas with the same dimensions as the SVG
        const canvas = document.createElement("canvas");
        const ctx = canvas.getContext("2d");

        canvas.width = img.width;
        canvas.height = img.height;

        // Draw the image onto the canvas
        ctx.drawImage(img, 0, 0);

        // Convert canvas to PNG blob
        canvas.toBlob((pngBlob) => {
          URL.revokeObjectURL(url);
          resolve(pngBlob);
        }, "image/png");
      };

      img.onerror = () => {
        URL.revokeObjectURL(url);
        reject(new Error("Failed to load SVG image"));
      };

      img.src = url;
    });
  },
};
