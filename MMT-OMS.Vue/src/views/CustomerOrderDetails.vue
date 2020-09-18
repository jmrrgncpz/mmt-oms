<template>
    <v-container>
        <v-row v-if="!!customerOrderDetails" justify="start">
            <v-col cols="3">
                <v-card shaped>
                    <v-card-title>{{ customerOrderDetails.brandName }}</v-card-title>
                    <v-card-subtitle>Brand</v-card-subtitle>
                </v-card>
            </v-col>
            <v-col cols="3">
                <v-card shaped>
                    <v-card-title> {{ customerOrderDetails.bundleCode || 'Custom' }} </v-card-title>
                    <v-card-subtitle>Bundle Code</v-card-subtitle>
                </v-card>
            </v-col>
            <v-col cols="3">
                <v-card shaped>
                    <v-card-title> {{ customerOrderDetails.uniqueCode }} </v-card-title>
                    <v-card-subtitle>Order Code</v-card-subtitle>
                </v-card>
            </v-col>
        </v-row>

        <v-row v-if="!!customerOrderDetails">
            <v-col>
                <v-stepper v-model="customerOrderDetails.orderStatusId">
                    <v-stepper-header>
                        <template
                            v-for="(status, key, i) in orderStatuses">
                            <v-stepper-step
                                v-bind:key="status.id"
                                v-bind:step="i + 1"
                                v-bind:complete="customerOrderDetails.orderStatusId > i + 1"
                                v-bind:editable="i + 1 == customerOrderDetails.orderStatusId + 1"
                                v-bind:complete-icon="'mdi-check'"
                                v-on:click="i + 1 == customerOrderDetails.orderStatusId + 1 && setOrderStatus(customerOrderDetails.orderStatusId + 1)">
                                {{ status.name }}
                            </v-stepper-step>

                            <v-divider
                                v-if="i + 1 !== Object.keys(orderStatuses).length"
                                v-bind:key="`divider-${i + 1}`"
                            ></v-divider>
                        </template>
                    </v-stepper-header>
                </v-stepper>
            </v-col>
        </v-row>
        <div id="shipping-information">
            <v-toolbar flat color="white">
                <v-toolbar-title>Shipping Information</v-toolbar-title>
            </v-toolbar>
            <v-row>
                <v-col cols="6">
                    <v-card v-if="!!customerOrderDetails">
                        <v-card-title>
                            Receiver
                        </v-card-title>
                        <v-card-title class="text-h5">
                            {{ customerOrderDetails.clientName }}
                        </v-card-title>
                        <v-card-subtitle>Name</v-card-subtitle>

                        <v-card-title class="text-h5">
                            {{ customerOrderDetails.phoneNumber }}
                        </v-card-title>
                        <v-card-subtitle>Contact Number</v-card-subtitle>

                        <v-card-title class="text-h5">
                            {{ customerOrderDetails.deliveryAddress }}
                        </v-card-title>
                        <v-card-subtitle>Address</v-card-subtitle>
                    </v-card>
                </v-col>
            </v-row>
        </div>
        
    </v-container>
</template>

<script>
import orderStatus from '@/order-status.js';

export default {
    data(){
        return {
            orderStatuses: orderStatus
        }
    },
    props: ['customerOrderDetails'],
    methods: {
        setOrderStatus(orderStatusId){
            const originalOrderId = this.customerOrderDetails.orderStatusId;
            const that = this;
            this.$axios({
                method: 'post',
                url: `api/customer-order/set-status`,
                crossdomain: false,
                data: {
                    CustomerOrderId: that.$route.params.id,
                    OrderStatusId: orderStatusId
                }
            }).then(() => {
                that.customerOrderDetails.orderStatusId = orderStatusId;
            }).catch(() => {
                that.customerOrderDetails.orderStatusId = originalOrderId;
            });
        }
    },
}
</script>