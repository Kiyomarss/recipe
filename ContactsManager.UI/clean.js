const { rimraf } = require('rimraf');

rimraf(".parcel-cache", { glob: false }, () => {
    rimraf("dist", { glob: false }, () => {
        console.log("Cache and dist folders cleaned!");
    });
});
