<template>
    <v-container>
        <v-data-table
            show-select
            item-key="UniqueCode"
            v-on:dblclick:row="openCustomerOrderDetails"
            class="no-print"
            v-bind:headers="headers"
            v-bind:items="customerOrders"
            v-bind:search="search"
            v-model="selectedOrders">
            <template v-slot:top>
                <v-toolbar flat color="white">
                    <v-toolbar-title>Customer Orders</v-toolbar-title>
                    <v-divider class="mx-4" inset vertical></v-divider>
                    <v-btn
                        icon
                        outlined class="purple--text"
                        v-bind:disabled="!selectedOrders.length"
                        v-on:click="printPackageLabel">
                        <v-icon>mdi-printer</v-icon>
                    </v-btn>
                    <v-spacer></v-spacer>
                    <v-text-field
                        v-model="search"
                        append-icon="mdi-magnify"
                        label="Search"
                        single-line
                        hide-details
                    ></v-text-field>
                </v-toolbar>
            </template>
            <template v-slot:item.OrderStatusName="props">
                <span>
                    {{ props.item.OrderStatusName | split }}
                </span>
            </template>
        </v-data-table>

        <div id="shipping-information" class="no-screen">
            <v-card class="pa-4 package-label" v-for="order in selectedOrders" v-bind:key="order.UniqueCode">
                <div class="receiver-information">
                    <div class="text-h5 info-header">Receiver</div>
                    <div class="d-flex justify-space-between">
                        <span>Name :</span> <span class="font-weight-black">{{ order.ClientName }}</span>
                    </div>
                    <div class="d-flex justify-space-between">
                        <span>Contact Number :</span> <span class="font-weight-black">{{ order.PhoneNumber }}</span>
                    </div>
                    <div>
                        <span>Address :</span> <span class="font-weight-black">{{ order.DeliveryAddress }}</span>
                    </div>
                </div>
                <v-divider class="my-4"></v-divider>
                <div class="sender-information">
                    <div class="text-h5 info-header">Sender</div>
                    <div class="d-flex justify-space-between">
                        <span>Name :</span> <span class="font-weight-black">Maureen Mae M. Sinen</span>
                    </div>
                    <div class="d-flex justify-space-between">
                        <span>Contact Number :</span> <span class="font-weight-black">9497138064</span>
                    </div>
                    <div>
                        <span>Address :</span> <span class="font-weight-black">B15 L4 Oakhill Subdv., Brgy. Palo Alto, Calamba, Laguna</span>
                    </div>
                </div>
            </v-card>
        </div>
    </v-container>
</template>

<script>
import { routeNames } from '@/router.js';

export default {
    data(){
        return {
            search: '',
            selectedOrders: [],
            customerOrders: [],
            headers : [
                {
                    text: 'Order Date',
                    value: 'OrderDate'
                },
                {
                    text : 'Brand Name',
                    value : 'BrandName'
                },
                {
                    text : 'Client Name',
                    value : 'ClientName'
                },
                {
                    text : 'Client FB Name',
                    value : 'ClientFacebookName'
                },
                {
                    text : 'Phone Number',
                    value : 'PhoneNumber'
                },
                {
                    text : 'Amount',
                    value : 'Amount'
                },
                {
                    text : 'Order Status',
                    value : 'OrderStatus'
                },
                {
                    text : 'Code',
                    value : 'UniqueCode'
                }
            ]
        }
    },
    methods: {
        getCustomerOrders(){
           return this.$axios({
               method : 'get',
               crossdomain: false,
               url: 'api/customer-order'
           }).then(res => res);
        },
        loadCustomerOrderList(){
            this.getCustomerOrders()
            .then(res => {
                this.customerOrders = res.data.map(row => {
                    return Object.assign(row, { BundleCode: row.BundleCode || 'Custom' })
                });
            });
        },
        openCustomerOrderDetails(event, row){
            this.$router.push(`customer-order/${row.item.Id}/details`);
        },
        printPackageLabel(){
            window.print();
        }
    },
    beforeMount(){
        this.loadCustomerOrderList();
    },
    filters: {
        split(word){
            const words = word.match(/[A-Z]*[^A-Z]+/g);
            return words.reduce((prev, curr) => prev + ' ' + curr, '');
        }
    },
    mounted(){
        this.$router.push({ name : routeNames.customerOrders });
    }
}
</script>

<style>
    .package-label{
        width: 30%;
        float: left;
        margin: 0 .2cm .2cm 0;
        page-break-inside: avoid;
    }
</style>