export const copyToClipboard = (data: any) => {
  const textToCopy = JSON.stringify(data, null, 2);

  navigator.clipboard
    .writeText(textToCopy)
    .then(() => {
      console.log("Settings copied to clipboard!");
    })
    .catch((err) => {
      console.error("Failed to copy settings:", err);
    });
};
