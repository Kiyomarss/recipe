import View from './View.js';
import icons from 'url:../../img/icons.svg';
import {AJAX} from "../helpers";
import {Register} from "../config";

class RegisterView extends View {
  _parentElement = document.querySelector('.register-upload');
  _message = 'Registration successful!';

  _window = document.querySelector('.register-window');
  _overlay = document.querySelector('.register-overlay');
  _btnClose = document.querySelector('.register-btn--close-modal');

  constructor() {
    super();
    this._addHandlerHideWindow();
  }

  toggleWindow() {
    this._overlay.classList.toggle('hidden');
    this._window.classList.toggle('hidden');
  }

  _addHandlerHideWindow() {
    this._btnClose.addEventListener('click', this.toggleWindow.bind(this));
    this._overlay.addEventListener('click', this.toggleWindow.bind(this));
  }

  async _register(registerDto) {
    try {
      const dto = {
        personName: registerDto.personName,
        email: registerDto.email,
        phone: registerDto.phone,
        password: registerDto.password,
        confirmPassword: registerDto.confirmPassword,
        userType: registerDto.userType,
      };

      const result = await AJAX(`${Register}`, dto);

      if (result.success) {
        this.toggleWindow();
      }
      else {
        const errorContainer = document.querySelector("#error-messages");
        errorContainer.innerHTML = result.errors.join("<br>");
        errorContainer.style.display = "block";
      }
    } catch (err) {
      const errorContainer = document.querySelector("#error-messages");
      errorContainer.innerHTML = err.join("<br>");
      errorContainer.style.display = "block";
    }
  }

  _addHandlerRegister(handler) {
    this._parentElement.addEventListener('submit', function (e) {
      e.preventDefault();
      const dataArr = [...new FormData(this)];
      const data = Object.fromEntries(dataArr);
      handler(data);
    });
  }

  _generateMarkup() {}
}

export default new RegisterView();
