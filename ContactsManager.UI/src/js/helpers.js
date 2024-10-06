import { async } from 'regenerator-runtime';
import {API_URL2, TIMEOUT_SEC} from './config.js';

const timeout = function (s) {
  return new Promise(function (_, reject) {
    setTimeout(function () {
      reject(new Error(`Request took too long! Timeout after ${s} second`));
    }, s * 1000);
  });
};

export const AJAX = async function (url, uploadData = undefined) {
  try {
      console.log(uploadData);
      const fetchPro = uploadData
          ? fetch(url, {
              method: 'POST',
              headers: {
                  'Content-Type': 'application/json',
              },
              body: JSON.stringify(uploadData),
          })
          : fetch(url);
      
    const res = await Promise.race([fetchPro, timeout(TIMEOUT_SEC)]);
    
      if (!res.ok) {
          // استخراج پیام خطا یا تولید یک پیام خطا
          const errorMessage = `Failed to fetch data. Status: ${res.status} ${res.statusText}`;
          throw new Error(errorMessage); // پرتاب خطا در صورت عدم موفقیت درخواست
      }
    
    const data = await res.json();
      console.log(data);
      
    return data;
  } catch (err) {
    throw err;
  }
};

/*
export const getJSON = async function (url) {
  try {
    const fetchPro = fetch(url);
    const res = await Promise.race([fetchPro, timeout(TIMEOUT_SEC)]);
    const data = await res.json();

    if (!res.ok) throw new Error(`${data.message} (${res.status})`);
    return data;
  } catch (err) {
    throw err;
  }
};

export const sendJSON = async function (url, uploadData) {
  try {
    const fetchPro = fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(uploadData),
    });

    const res = await Promise.race([fetchPro, timeout(TIMEOUT_SEC)]);
    const data = await res.json();

    if (!res.ok) throw new Error(`${data.message} (${res.status})`);
    return data;
  } catch (err) {
    throw err;
  }
};
*/
