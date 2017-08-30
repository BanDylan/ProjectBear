/// <reference path="../Common/Helpers/LanguageList.ts" />

namespace ProjectBear.CMS.Web.ScriptInitialization {
    Q.Config.responsiveDialogs = true;
    Q.Config.rootNamespaces.push('ProjectBear.CMS.Web');
    Serenity.EntityDialog.defaultLanguageList = LanguageList.getValue;
}