const environmentArg = process.argv.find((i) => i.startsWith('--environment='));
if (environmentArg === null) {
  throw new Error(`No environment specified`);
}
const environment = environmentArg.replace('--environment=', '');
