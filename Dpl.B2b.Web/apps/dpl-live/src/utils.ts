import { UrlHandlingStrategy } from '@angular/router';

const localePath = (() => {
  const pathMatch = window.location.pathname.match(/^(\/.{2}\/)/);
  return pathMatch ? pathMatch[0] : '/';
})();

export const isIE =
  window.navigator.userAgent.indexOf('MSIE ') > -1 ||
  window.navigator.userAgent.indexOf('Trident/') > -1;

export function isMsalIframe() {
  return window.self !== window.top;
}

export function localizeUrl(urlString: string) {
  if (urlString.match(/\:\/\//)) {
    const url = new URL(urlString);
    // ensure path is at least /
    url.pathname += url.pathname.length === 0 ? '/' : '';
    urlString = url.toString();
  }

  return urlString.replace(/(?<![\:|\/])\//, localePath);
}
