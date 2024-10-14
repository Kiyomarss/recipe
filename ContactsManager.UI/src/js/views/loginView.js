import View from './View.js';
import icons from 'url:../../img/icons.svg';
import { AJAX } from "../helpers";
import { Login, Logout } from "../config";

class LoginView extends View {
  _parentElement = document.querySelector('.login-upload');
  _message = 'Login successful!';

  _window = document.querySelector('.login-window');
  _overlay = document.querySelector('.login-overlay');
  _btnOpen = document.querySelector('.nav__btn--login'); // دکمه لاگین (که به لاگ‌اوت تغییر می‌کند)
  _btnClose = document.querySelector('.login-btn--close-modal');
  _btnRegister = document.querySelector('.register-btn');
  _isLoggedIn = false; // وضعیت ورود کاربر

  constructor() {
    super();
    this._checkLoginStatus(); // بررسی وضعیت ورود در ابتدای لود صفحه
    this._addHandlerShowWindow();
    this._addHandlerHideWindow();
    this._addHandlerRegisterOpen();
  }

  // بررسی وضعیت لاگین در SessionStorage هنگام بارگذاری صفحه
  _checkLoginStatus() {
    const loggedIn = sessionStorage.getItem('isLoggedIn');
    if (loggedIn === 'true') {
      this._isLoggedIn = true;
      this._changeLoginToLogout(); // تغییر وضعیت دکمه به لاگ‌اوت
    } else {
      this._isLoggedIn = false;
      this._changeLogoutToLogin(); // تغییر وضعیت دکمه به لاگین
    }
  }

  toggleWindow() {
    const isOpen = !this._window.classList.contains('hidden');

    if (isOpen) {
      this.resetModal();
    }

    this._overlay.classList.toggle('hidden');
    this._window.classList.toggle('hidden');
  }

  _addHandlerRegisterOpen() {
    this._btnRegister.addEventListener('click', () => {
      this.toggleWindow();
      document.querySelector('.register-window').classList.remove('hidden');
      document.querySelector('.register-overlay').classList.remove('hidden');
    });
  }

  _addHandlerShowWindow() {
    this._btnOpen.addEventListener('click', () => {
      if (!this._isLoggedIn) {
        this.toggleWindow();
      } else {
        this._logout();  // فراخوانی تابع لاگ‌اوت در حالت لاگین
      }
    });
  }

  _addHandlerHideWindow() {
    this._btnClose.addEventListener('click', this.toggleWindow.bind(this));
    this._overlay.addEventListener('click', this.toggleWindow.bind(this));
  }

  _addHandlerLogin(handler) {
    this._parentElement.addEventListener('submit', function (e) {
      e.preventDefault();
      const dataArr = [...new FormData(this)];
      const data = Object.fromEntries(dataArr);
      handler(data);
    });
  }

  async _login(loginDto) {
    const dto = {
      email: loginDto.userName,
      password: loginDto.password,
    };

    const result = await AJAX(`${Login}`, dto);

    if (!result.success) {
      this._errorMessage = result.errors.join("<br>");
      return false;
    }

    this._message = 'Login successful!';
    this._isLoggedIn = true;
    sessionStorage.setItem('isLoggedIn', 'true');  // ذخیره وضعیت لاگین در SessionStorage
    this._changeLoginToLogout();

    return true;
  }

  _changeLoginToLogout() {
    this._btnOpen.textContent = 'Logout';  // تغییر متن دکمه به لاگ‌اوت
    this._isLoggedIn = true;  // به‌روزرسانی وضعیت ورود
  }

  async _logout() {
    const result = await AJAX(`${Logout}`);

    if (result.success) {
      this._message = 'Logout successful!';
      this.renderMessage(this._message);
      sessionStorage.removeItem('isLoggedIn'); // حذف وضعیت لاگین از SessionStorage
      this._changeLogoutToLogin();
    } else {
      this._errorMessage = result.error;
    }
  }

  _changeLogoutToLogin() {
    this._btnOpen.textContent = 'Login';  // تغییر متن دکمه به لاگین
    this._isLoggedIn = false;
  }

  _generateMarkup() {}
}

export default new LoginView();
