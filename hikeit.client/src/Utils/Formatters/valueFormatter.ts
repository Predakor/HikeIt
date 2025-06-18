import { dateOnlyToString } from "./dateFormats";

export function GenericFormatter<T extends string | number>(value: T) {
  switch (typeof value) {
    case "string":
      return isValidDateString(value) ? dateOnlyToString(value) : value;

    case "number":
      return value.toFixed(0);
  }
}

export function NumberFormatter(value: number, decimals = 1) {
  return value.toFixed(decimals);
}

export function KeyToLabelFormatter(key: string) {
  const detectedWords: string[] = [];

  let lastWordIndex = 0;
  for (let i = 1; i < key.length; i++) {
    const char = key.charAt(i);

    if (isUpperCase(char)) {
      const word = key.slice(lastWordIndex, i).toLowerCase();
      detectedWords.push(word);
      lastWordIndex = i;
    }
  }

  let lastWord = key.slice(lastWordIndex);

  //lower case last word
  const hasMultipleWords = lastWordIndex != 0;
  if (hasMultipleWords) {
    lastWord = lastWord.toLowerCase();
  }

  detectedWords.push(lastWord);

  //capitalize first word
  detectedWords[0] = CapitalizeWord(detectedWords[0]);

  return detectedWords.join(" ");
}

function CapitalizeWord(detectedWords: string) {
  return detectedWords.charAt(0).toUpperCase() + detectedWords.slice(1);
}

function isUpperCase(char: string) {
  return char.toUpperCase() === char && char !== char.toLowerCase();
}

function isValidDateString(value: string): boolean {
  const date = new Date(value);
  return !isNaN(date.getTime());
}
