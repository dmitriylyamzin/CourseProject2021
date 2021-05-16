import random
import numpy as np

def appendInputData (line, odd_data_flag):
    result = []
    for i in range (len(line)):
        if odd_data_flag == 1:
            if i != 0 and i != 8:
                result.append(activationFunction(int(line[i])))
        else:
            result.append(activationFunction(int(line[i])))
    return result

def getInputFriendsData ():
    input_nodes = []
    file1 = open("friends_database.txt","r")
    file2 = open("friends2_database.txt","r")
    file3 = open("friends3_database.txt","r")
    readlines1 = file1.readlines()
    readlines2 = file2.readlines()
    readlines3 = file3.readlines()
    for number in range (15000):
        input_nodes.append(appendInputData (readlines1[number].split(), 1) + appendInputData (readlines2[number].split(), 0) + appendInputData (readlines3[number].split(), 0))
    file3.close()     
    file2.close()     
    file1.close()    
    return input_nodes

def getInputFollowersData ():
    input_nodes = []
    file1 = open("followers_database.txt","r")
    file2 = open("followers2_database.txt","r")
    file3 = open("followers3_database.txt","r")
    readlines1 = file1.readlines()
    readlines2 = file2.readlines()
    readlines3 = file3.readlines()
    for number in range (15000):
        input_nodes.append(appendInputData (readlines1[number].split(), 1) + appendInputData (readlines2[number].split(), 0) + appendInputData (readlines3[number].split(), 0))
    file3.close()     
    file2.close()     
    file1.close()    
    return input_nodes

def setWeights ():
    weights = []
    for i in range (58):
        weights.append([])
        for j in range (58):
            weights[i].append(np.random.normal())
    return weights

def setFinalWeights ():
    return [np.random.normal() for i in range (58)]    

def setBiasWeights ():
    return [np.random.normal() for i in range (58)]


def activationFunction (x):
    return 1 / (1 + np.exp(-x))

def derivative (x):
    return x * (1 - x)

def countMSE (era_results):
    total_error = 0
    result_number = 0
    for result in era_results:
        if result_number < 15000:
            expected_result = 1
        else:
            expected_result = 0
        result_number += 1
        total_error += (result - expected_result) ** 2
    return total_error/len(era_results)

def getNextInnerLayer (current_nodes, current_weights, bias_weights, af_refuse = False):
    next_nodes = [0 for i in range (58)]
    for j in range (58):
        for i in range (58):
            next_nodes[j] += current_nodes[i] * current_weights[i][j]
        next_nodes[j] += bias_weights[j]
        if not af_refuse:
            next_nodes[j] = activationFunction(next_nodes[j])
    
    return next_nodes

def getOutputLayer (current_nodes, final_weights, bias_weight):
    output_node = 0
    for i in range (58):
        output_node += current_nodes[i] * final_weights[i]
    output_node += bias_weight
    return output_node

def findFinalWeightsDerivative (current_nodes, output):
    result = []
    output = activationFunction (output)
    for node in current_nodes:
        result.append (node * derivative (output))
    return result

def findWeightsDerivative (current_nodes, output_nodes):
    result = []
    for i in range (len(current_nodes)):
        result.append([])
        for j in range (len(current_nodes)):
            result[i].append(current_nodes[j] * derivative (activationFunction (output_nodes[i])))
    return result

def findFinalBiasDerivative (output):
    output = activationFunction (output)
    return derivative (output)

def findBiasDerivative (output_nodes):
    return [derivative (activationFunction (output_nodes[i])) for i in range (len (output_nodes))]

def findFinalPrevNodesDerivative(weights, output):
    output = activationFunction (output)
    return [derivative (output) * weights[i] for i in range (len (weights))]

def findPrevNodesDerivative(weights, output_nodes):
    result = [0 for i in range (len (output_nodes))]
    for i in range (len(output_nodes)):
        for j in range (len(output_nodes)):
            result[i] += weights[i][j] * derivative (activationFunction (output_nodes[j]))
    return result    

def updateFinalWeights (weights, learn_rate, main_derivative, d_w):
    return [weights[i] - learn_rate * main_derivative * d_w[i] for i in range (len (weights))]

def updateWeights(weights, learn_rate, main_derivative, d_n, d_w):
    for j in range (len(weights)):
        for i in range (len (weights)):
            weights[i][j] -= learn_rate * main_derivative * d_n[j] * d_w[i][j]
    return weights
            
def updateBiasWeights (bias_weights, learn_rate, main_derivative, d_n, d_b):
    return [bias_weights[i] - learn_rate * main_derivative * d_n[i] * d_b[i] for i in range (len (bias_weights))]

class FriendShipperNeuralNetwork:
    def __init__(self):
        self.first_weights = setWeights()
        self.second_weights = setWeights()
        self.third_weights = setWeights()
        self.fourth_weights = setFinalWeights()
        self.first_bias_weights = setBiasWeights()
        self.second_bias_weights = setBiasWeights()
        self.third_bias_weights = setBiasWeights()
        self.fourth_bias_weight = np.random.normal()
        self.min_error = 100
        self.min_w1 = self.first_weights
        self.min_w2 = self.second_weights
        self.min_w3 = self.third_weights
        self.min_w4 = self.fourth_weights
        self.min_b1 = self.first_bias_weights
        self.min_b2 = self.second_bias_weights
        self.min_b3 = self.third_bias_weights
        self.min_b4 = self.fourth_bias_weight        
        
        
    def feedforward(self, current_nodes):
        current_nodes = getNextInnerLayer (current_nodes, self.first_weights, self.first_bias_weights)
        current_nodes = getNextInnerLayer (current_nodes, self.second_weights, self.second_bias_weights)
        current_nodes = getNextInnerLayer (current_nodes, self.third_weights, self.third_bias_weights)
        output = activationFunction (getOutputLayer (current_nodes, self.fourth_weights, self.fourth_bias_weight))
        return output
        
    def train(self, data):
        learn_rate = 0.0003
        count_of_eras = 1000
        for era in range(count_of_eras):
            
            # Запускаем счётчик количества пройденных наборов данных
            line_number = -1
            for current_nodes in data:
                line_number += 1
                
                # Находим выходные значения нейронов всех слоёв по методу прямого распространения 
                #(до и после применения функции активации)
                result1 = getNextInnerLayer (current_nodes, self.first_weights, self.first_bias_weights, True)
                current_nodes_1 = [activationFunction (result1[i]) for i in range (len(result1))]
                result2 = getNextInnerLayer (current_nodes_1, self.second_weights, self.second_bias_weights, True)
                current_nodes_2 = [activationFunction (result2[i]) for i in range (len(result2))]
                result3 = getNextInnerLayer (current_nodes_2, self.third_weights, self.third_bias_weights, True)
                current_nodes_3 = [activationFunction (result3[i]) for i in range (len(result3))]
                output = getOutputLayer (current_nodes_3, self.fourth_weights, self.fourth_bias_weight)
                predicted_output = activationFunction (output)
                expected_output = 0
                # Первая половина строк в файле с данными - друзья, вторая - подписчики,
                #поэтому для первой половины ожидаемый результат равен 1, для второй - 0
                if line_number < 15000:
                    expected_output = 1
                
                # Вычисляем производные, необходимые для реализации метода обратного распространения. 
                main_derivative = -2 * (expected_output - predicted_output)
                d_ypred_d_w4 = findFinalWeightsDerivative (current_nodes_3, output)
                d_ypred_d_b4 = findFinalBiasDerivative (output)
                d_ypred_d_n4 = findFinalPrevNodesDerivative(self.fourth_weights, output)
                
                d_w4_d_w3 = findWeightsDerivative (current_nodes_2, result3)
                d_b4_d_b3 = findBiasDerivative (result3)
                d_n4_d_n3 = findPrevNodesDerivative(self.third_weights, result3)

                d_w3_d_w2 = findWeightsDerivative (current_nodes_1, result2)
                d_b3_d_b2 = findBiasDerivative (result2)
                d_n3_d_n2 = findPrevNodesDerivative(self.second_weights, result2)
 
                d_w2_d_w1 = findWeightsDerivative (current_nodes, result1)
                d_b2_d_b1 = findBiasDerivative (result1)
                
                # Обновляем веса, отнимая из них произведение скорости обучения на найденные производные, переданные в параметрах к методам
                self.first_weights = updateWeights(self.first_weights, learn_rate, main_derivative, d_n3_d_n2, d_w2_d_w1)
                self.second_weights = updateWeights(self.second_weights, learn_rate, main_derivative, d_n4_d_n3, d_w3_d_w2)
                self.third_weights = updateWeights(self.third_weights, learn_rate, main_derivative, d_ypred_d_n4, d_w4_d_w3)
                self.fourth_weights = updateFinalWeights(self.fourth_weights, learn_rate, main_derivative, d_ypred_d_w4)     
         
                self.first_bias_weights = updateBiasWeights(self.first_bias_weights, learn_rate, main_derivative, d_n3_d_n2, d_b2_d_b1)
                self.second_bias_weights = updateBiasWeights(self.second_bias_weights, learn_rate, main_derivative, d_n4_d_n3, d_b3_d_b2)
                self.third_bias_weights = updateBiasWeights(self.third_bias_weights, learn_rate, main_derivative, d_ypred_d_n4, d_b4_d_b3)
                self.fourth_bias_weight -= learn_rate * main_derivative * d_ypred_d_b4              

            # Получаем результаты работы нейросети с текущими весами для всего датасета и считаем ошибку, запоминая новые оптимальные веса, если она уменьшилась
            # Далее печатаем результат обучения на данной эпохе и при необходимости переходим к следующей
            era_results = np.apply_along_axis(self.feedforward, 1, data)
            print (era_results)
            error = countMSE(era_results)
            if error < self.min_error:
                self.min_error = error
                self.min_w1 = self.first_weights
                self.min_w2 = self.second_weights
                self.min_w3 = self.third_weights
                self.min_w4 = self.fourth_weights
                self.min_b1 = self.first_bias_weights
                self.min_b2 = self.second_bias_weights
                self.min_b3 = self.third_bias_weights
                self.min_b4 = self.fourth_bias_weight
                
            print("Era %d error: %.3f" % (era, error))

data = np.array(getInputFriendsData() + getInputFollowersData(), dtype = np.float64)
fs_network = FriendShipperNeuralNetwork()
fs_network.train(data)
print (fs_network.min_error)
print (fs_network.min_w1)
print (fs_network.min_w2)
print (fs_network.min_w3)
print (fs_network.min_w4)
print (fs_network.min_b1)
print (fs_network.min_b2)
print (fs_network.min_b3)
print (fs_network.min_b4)