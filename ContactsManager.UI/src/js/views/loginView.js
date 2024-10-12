import View from './View.js';
import icons from 'url:../../img/icons.svg';
import {AJAX} from "../helpers";
import {Login} from "../config"; // Parcel 2

class LoginView extends View {
  _parentElement = document.querySelector('.login-upload');
  _message = 'Login successful!';

  _window = document.querySelector('.login-window');
  _overlay = document.querySelector('.login-overlay');
  _btnOpen = document.querySelector('.nav__btn--login');
  _btnClose = document.querySelector('.login-btn--close-modal');
  _btnRegister = document.querySelector('.register-btn');

  constructor() {
    super();
    this._addHandlerShowWindow();
    this._addHandlerHideWindow();
    this._addHandlerRegisterOpen();
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
    this._btnOpen.addEventListener('click', this.toggleWindow.bind(this));
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
    }
    
    return result.success;
  }

  _generateMarkup() {}
}

export default new LoginView();
