<template>
    <v-container>
        <div id="interface">
            <v-data-table
                v-bind:headers="headers"
                v-bind:items="pool"
                v-bind:items-per-page="5"
                item-key="customerOrderShadeId"
                show-select
                show-group-by
                v-model="bucket">
                <template v-slot:top>
                    <v-toolbar flat color="white" id="print-table-header">
                        <v-toolbar-title>Print Layouts</v-toolbar-title>
                        <v-divider
                            class="mx-4"
                            inset
                            vertical
                        ></v-divider>
                        <v-btn
                            outlined class="purple--text"
                            v-bind:disabled="!bucket.length"
                            v-on:click="printPreview">
                            Print Preview
                        </v-btn>
                    </v-toolbar>
                </template>
                <template v-slot:item.layoutImageFilePath="props">
                    <v-img v-bind:src="`${props.item.layoutImageFilePath}`" max-width="100"></v-img>
                </template>
                <template v-slot:item.quantity="props">
                    <v-text-field
                        solo flat
                        type="number"
                        class="pa-4"
                        v-model="props.item.quantity"></v-text-field>
                </template>
            </v-data-table>
        </div>
        <div id="results-container">
            <img
                v-for="(layout, n) in staged"
                v-bind:key="`${n}-${layout.customerOrderShadeId}`"
                v-bind:class="[
                    layout.tintTypeId == 1 ? 'is-gel' : 'is-powder'
                ]"
                v-bind:src="`${layout.layoutImageFilePath}`">
        </div>
    </v-container>
</template>

<script>
export default {
    data(){
        return {
            pool: [],
            headers: [
                {
                    text: 'Original Shade Name',
                    value: 'shadeName',
                    groupable: false
                },
                {
                    text: 'Custom Shade Name',
                    value: 'customShadeName',
                    groupable: false
                },
                {
                    text: 'Quantity',
                    value: 'quantity',
                    groupable: false
                },
                {
                    text: 'Tint Type',
                    value: 'tintType'
                },
                {
                    text: 'Client | Brand',
                    value: 'client',
                },
                {
                    text: 'Layout',
                    value: 'layoutImageFilePath',
                    sortable: false,
                    filterable: false,
                    groupable: false
                }
            ],
            bucket : [],
            staged: []
        }
    },
    methods: {
        printPreview(){
            const staged = [];
            const gels = this.bucket.filter(l => l.tintTypeId == 1);
            const matte = this.bucket.filter(l => l.tintTypeId == 2);

            gels.forEach(layout => {
                for(let i = 0; i < layout.quantity; i++){
                    staged.push(layout);
                }
            });

            matte.forEach(layout => {
                for(let i = 0; i < layout.quantity; i++){
                    staged.push(layout);
                }
            });

            this.staged = staged;

            this.$nextTick(() => {
                window.print();
            });
        }
    },
    beforeMount(){
        const that = this;
        this.$axios({
            method: 'get',
            crossdomain: false,
            url : `api/shade-layouts`
        }).then(res => {
            that.pool = res.data.map(s => {
                return {
                    customerOrderShadeId: s.CustomerOrderShadeId,
                    customerOrderId: s.CustomerOrderId,
                    shadeId: s.ShadeId,
                    quantity: s.Quantity,
                    shadeName: s.ShadeName,
                    customShadeName: s.CustomShadeName,
                    layoutImageFilePath: s.LayoutImageFilePath,
                    tintType: s.TintType,
                    tintTypeId: s.TintTypeId,
                    client : s.CustomerOrder.BrandName + ' | ' + s.CustomerOrder.ClientFacebookName,
                    uniqueCode: s.CustomerOrder.UniqueCode
                }
            });
        });
    }
}
</script>

<style scoped>
    #print-table-header{
        position: sticky;
        top: 0;
        z-index: 10;
        background: white;
    }

    .is-gel{
        height: 4.87cm;
        width: 6.39cm;
    }

    .is-powder{
        height: 6.16cm;
        width: 4.64cm;
    }
    
    @media screen {
        #results-container{
            display: none;
        }
    }
    
    @media print{
        #interface{
            display: none;
        }

        .is-gel{
            margin-top: .1cm;
            margin-left: .2cm;
        }

        .is-gel:not(:last-child){
            margin-right: .2cm;
        }

        .is-powder{
            margin-top: .1cm;
        }

        .is-powder:not(:last-child){
            margin-right: .2cm;
        }
    }
</style>