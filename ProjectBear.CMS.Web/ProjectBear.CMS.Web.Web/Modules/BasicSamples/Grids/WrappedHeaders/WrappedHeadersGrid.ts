/// <reference path="../../../Northwind/Order/OrderGrid.ts" />

namespace ProjectBear.CMS.Web.BasicSamples {

    @Serenity.Decorators.registerClass()
    export class WrappedHeadersGrid extends Northwind.OrderGrid {

        constructor(container: JQuery) {
            super(container);
        }
    }
}