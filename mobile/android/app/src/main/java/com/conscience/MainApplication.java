package com.conscience;

import android.app.Application;

import com.facebook.react.ReactApplication;
import com.ironsmile.RNWakeful.RNWakefulPackage;
import com.microsoft.codepush.react.CodePush;
import com.github.wumke.RNExitApp.RNExitAppPackage;
import com.oblador.keychain.KeychainPackage;
import com.imagepicker.ImagePickerPackage;
import com.horcrux.svg.SvgPackage;
import com.oblador.vectoricons.VectorIconsPackage;
import com.zmxv.RNSound.RNSoundPackage;
import com.rnfs.RNFSPackage;
import com.learnium.RNDeviceInfo.RNDeviceInfo;
import com.robinpowered.react.battery.DeviceBatteryPackage;
import com.ocetnik.timer.BackgroundTimerPackage;
import com.facebook.react.ReactNativeHost;
import com.facebook.react.ReactPackage;
import com.facebook.react.shell.MainReactPackage;
import com.facebook.soloader.SoLoader;

import java.util.Arrays;
import java.util.List;

public class MainApplication extends Application implements ReactApplication {

  private final ReactNativeHost mReactNativeHost = new ReactNativeHost(this) {

        @Override
        protected String getJSBundleFile() {
        return CodePush.getJSBundleFile();
        }
    
    @Override
    public boolean getUseDeveloperSupport() {
      return BuildConfig.DEBUG;
    }

    @Override
    protected List<ReactPackage> getPackages() {
      return Arrays.<ReactPackage>asList(
          new MainReactPackage(),
            new RNWakefulPackage(),
            new CodePush(getResources().getString(R.string.reactNativeCodePush_androidDeploymentKey), getApplicationContext(), BuildConfig.DEBUG),
            new RNExitAppPackage(),
            new KeychainPackage(),
            new ImagePickerPackage(),
            new SvgPackage(),
            new VectorIconsPackage(),
            new RNSoundPackage(),
            new RNFSPackage(),
            new RNDeviceInfo(),
            new DeviceBatteryPackage(),
            new BackgroundTimerPackage()
      );
    }
  };

  @Override
  public ReactNativeHost getReactNativeHost() {
    return mReactNativeHost;
  }

  @Override
  public void onCreate() {
    super.onCreate();
    SoLoader.init(this, /* native exopackage */ false);
  }
}
