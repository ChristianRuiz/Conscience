export default function formatDate(dateString) {
  return new Date(dateString).toLocaleString()
  .replace('/2015', '/2055')
  .replace('/2016', '/2056')
  .replace('/2017', '')
  .replace('/2018', '');
}
