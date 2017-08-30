/// <reference path="../Common/Helpers/LanguageList.ts" />

namespace ProjectBear.CMS.ScriptInitialization {
    Q.Config.responsiveDialogs = true;
    Q.Config.rootNamespaces.push('ProjectBear.CMS');
    Serenity.EntityDialog.defaultLanguageList = LanguageList.getValue;
}