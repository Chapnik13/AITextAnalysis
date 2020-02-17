const posTagger = require('wink-pos-tagger');
const text = process.argv[2];
const tagger = posTagger();
const result = tagger.tagSentence(text).map(({ value, pos }) => ({value, pos}));

console.log(JSON.stringify(result));